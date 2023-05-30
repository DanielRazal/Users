using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users_Server.Enum;
using Users_Server.ViewModels;

namespace Users_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IJwtTokenGenerator _token;
        private readonly IUploadPhotos _uploads;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        public UserController(IUserRepository repo, IJwtTokenGenerator token,
         IUploadPhotos uploads, IEmailSender emailSender, IConfiguration config)
        {
            _repo = repo;
            _token = token;
            _uploads = uploads;
            _emailSender = emailSender;
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _repo.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById(int id)
        {
            var _user = await _repo.GetUserById(id);
            if (_user == null)
            {
                return NotFound($"Id: '{id}' not exist");
            }

            return Ok(_user);
        }

        [HttpPost("registration")]
        public async Task<ActionResult<User>> AddUser([FromForm] UserViewModel viewModel)
        {
            var _user = new User
            {
                FirstName = viewModel.UserDTO.FirstName,
                LastName = viewModel.UserDTO.LastName,
                UserName = viewModel.UserDTO.UserName,
                Password = viewModel.UserDTO.Password,
                Email = viewModel.UserDTO.Email
            };

            bool userNameExists = await _repo.UserNameExists(_user.UserName);
            bool emailExists = await _repo.EmailExists(_user.Email);

            if (userNameExists && emailExists)
            {
                return Unauthorized($"Username {_user.UserName} and Email {_user.Email} already exist");
            }
            else if (userNameExists)
            {
                return Unauthorized($"Username {_user.UserName} already exists");
            }
            else if (emailExists)
            {
                return Unauthorized($"Email {_user.Email} already exists");
            }

            var photo = await _uploads.UploadFile(_user, viewModel.Photo);

            _user.PhotoUrl = photo;

            var newUser = await _repo.AddUser(_user);

            await _emailSender.SendEmail(newUser, _config["SendGrid:SubjectForRegister"]!,
                _config["SendGrid:ContentForRegister"]!);

            return Ok(new { message = "You have successfully registered", StatusCode = 200 });
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateUser([FromForm] UpdateUserViewModel viewModel, int id)
        {
            var existingUser = await _repo.GetUserById(id);

            if (existingUser == null)
            {
                return NotFound($"Id: '{id}' not exist");
            }

            var user = new User
            {
                Id = existingUser.Id,
                FirstName = viewModel.UpdateUser.FirstName,
                LastName = viewModel.UpdateUser.LastName,
                Password = viewModel.UpdateUser.Password,
                UserName = existingUser.UserName,
                Email = existingUser.Email
            };

            var filePath = Path.GetFileName(existingUser.PhotoUrl);

            var photo = await _uploads.UpdateFile(user, filePath, viewModel.Photo);
            user.PhotoUrl = photo;

            var updateUser = await _repo.UpdateUser(user!, user.Id);

            await _emailSender.SendEmail(updateUser, _config["SendGrid:SubjectForUpdate"]!,
            string.Format(_config["SendGrid:ContentForUpdate"]!, updateUser.FirstName, updateUser.LastName));


            return Ok(new { Message = "Your details have been successfully updated", StatusCode = 200, user });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var _user = await _repo.DeleteUser(id);

            if (_user == null)
            {
                return NotFound($"Id: '{id}' does not exist");
            }

            var filePath = Path.GetFileName(_user.PhotoUrl);
            await _uploads.DeleteFile(filePath);

            return Ok(_user);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllUsers()
        {
            await _repo.DeleteAllUsers();
            await _uploads.DeleteAllFiles();
            return Ok(new { message = "All Users are deleted" });
        }



        [HttpPost("login")]
        public async Task<ActionResult> Login(Login login)
        {
            var user = await _repo.GetUserByUserName(login.UserName);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");

            }

            var _user = new User
            {
                UserName = login.UserName,
                Password = login.Password,
                PhotoUrl = user.PhotoUrl,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Role = user.Role
            };

            var isAuthenticated = await _repo.Login(_user);

            if (isAuthenticated)
            {
                var token = _token.GenerateToken(_user);
                return Ok(new { token, _user, message = "Login successful", StatusCode = 200 });
            }
            else
            {
                return Unauthorized("Invalid username or password");
            }
        }

        [HttpPost("details")]
        public async Task<ActionResult<object>> SendDetailsToEmail(string email)
        {
            var emailExists = await _repo.EmailExists(email);

            if (!emailExists)
            {
                return BadRequest($"Email {email} does not exist");
            }

            var user = await _repo.GetUserByEmail(email);


            await _emailSender.SendEmail(user, _config["SendGrid:SubjectForForgotPassword"]!,
                string.Format(_config["SendGrid:ContentForForgotPassword"]!, user.UserName, user.Password));

            return Ok(new { message = "Your Username and Password have been sent to your email", StatusCode = 200 });
        }

        // [HttpPatch("assign-role")]
        // // [Authorize(Roles = "ADMIN")]
        // public async Task<ActionResult> AssignRole(int id, UserRole userRole)
        // {
        //     var user = await _repo.GetUserById(id);

        //     if (user == null)
        //     {
        //         return NotFound($"User with ID: '{id}' not found");
        //     }

        //     user.Role = userRole;

        //     var userUpdated = await _repo.UpdateUser(user, id);

        //     return Ok(userUpdated);
        // }

        // [Authorize(Policy = "AdminOnly")]
        [HttpPatch("assign-role")]
        public async Task<ActionResult> AssignRole(int id, UserRole userRole)
        {
            var user = await _repo.GetUserById(id);

            if (user == null)
            {
                return NotFound($"Id: '{id}' not exist");
            }

            user.Role = userRole;

            var updated = await _repo.UpdateUser(user, id);

            return Ok(updated);
        }

    }
}