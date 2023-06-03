// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using SendGrid;
// using System.Net;
// using Users_Server.Controllers;
// using Users_Server.Models;
// using Users_Server.Repositories;
// using Users_Server.Services;
// using Users_Server.ViewModels;

// namespace UnitTest
// {
//     public class UnitTests
//     {
//         [Fact]
//         public async Task GetAllUsers_Returns_OkResult_With_Users()
//         {
//             // Arrange
//             var expectedUsers = new List<User>
//     {
//         new User { Id = 1, FirstName = "John", LastName = "Doe", UserName = "johndoe" },
//         new User { Id = 2, FirstName = "Jane", LastName = "Smith", UserName = "janesmith" }
//     };

//             var userRepositoryMock = new Mock<IUserRepository>();
//             userRepositoryMock.Setup(repo => repo.GetAllUsers()).Returns(Task.FromResult(expectedUsers));

//             var controller = new UserController(userRepositoryMock.Object, null!, null!, null!, null!);

//             // Act
//             var result = await controller.GetAllUsers();

//             // Assert
//             Assert.IsType<OkObjectResult>(result);
//             var okResult = (OkObjectResult)result;
//             var returnedUsers = (List<User>)okResult.Value!;

//             Assert.Equal(expectedUsers.Count, returnedUsers!.Count);
//             for (int i = 0; i < expectedUsers.Count; i++)
//             {
//                 Assert.Equal(expectedUsers[i].Id, returnedUsers[i].Id);
//                 Assert.Equal(expectedUsers[i].FirstName, returnedUsers[i].FirstName);
//                 Assert.Equal(expectedUsers[i].LastName, returnedUsers[i].LastName);
//                 Assert.Equal(expectedUsers[i].UserName, returnedUsers[i].UserName);
//             }
//         }

//         [Fact]
//         public async Task GetUserById_ExistingId_Returns_OkResult_With_User()
//         {
//             // Arrange
//             int userId = 1;
//             var expectedUser = new User { Id = userId, FirstName = "John", LastName = "Doe", UserName = "johndoe" };

//             var userRepositoryMock = new Mock<IUserRepository>();
//             userRepositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(Task.FromResult(expectedUser));

//             var controller = new UserController(userRepositoryMock.Object, null!, null!, null!, null!);

//             // Act
//             var result = await controller.GetUserById(userId);

//             // Assert
//             Assert.IsType<OkObjectResult>(result);
//             var okResult = (OkObjectResult)result;
//             var returnedUser = (User)okResult.Value!;

//             Assert.Equal(expectedUser.Id, returnedUser.Id);
//             Assert.Equal(expectedUser.FirstName, returnedUser.FirstName);
//             Assert.Equal(expectedUser.LastName, returnedUser.LastName);
//             Assert.Equal(expectedUser.UserName, returnedUser.UserName);
//         }

//         [Fact]
//         public async Task GetUserById_NonExistingId_Returns_NotFoundResult()
//         {
//             // Arrange
//             int userId = 999; // An ID that does not exist in the mock repository

//             var userRepositoryMock = new Mock<IUserRepository>();
//             userRepositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(Task.FromResult<User>(null!));

//             var controller = new UserController(userRepositoryMock.Object, null!, null!, null!, null!);

//             // Act
//             var result = await controller.GetUserById(userId);

//             // Assert
//             Assert.IsType<NotFoundObjectResult>(result);
//             var notFoundResult = (NotFoundObjectResult)result;
//             Assert.Equal($"Id: '{userId}' not exist", notFoundResult.Value);
//         }

//         [Fact]
//         public async Task DeleteUser_ExistingId_Returns_OkResult_With_DeletedUser()
//         {
//             // Arrange
//             int userId = 1;
//             var expectedUser = new User { Id = userId, FirstName = "John", LastName = "Doe", UserName = "johndoe", PhotoUrl = "photo.jpg" };

//             var userRepositoryMock = new Mock<IUserRepository>();
//             userRepositoryMock.Setup(repo => repo.DeleteUser(userId)).Returns(Task.FromResult(expectedUser));

//             var uploadPhotosMock = new Mock<IUploadPhotos>();
//             uploadPhotosMock.Setup(upload => upload.DeleteFile(expectedUser.PhotoUrl)).Returns(Task.CompletedTask);

//             var controller = new UserController(userRepositoryMock.Object, null!, uploadPhotosMock.Object, null!, null!);

//             // Act
//             var result = await controller.DeleteUser(userId);

//             // Assert
//             Assert.IsType<OkObjectResult>(result);
//             var okResult = (OkObjectResult)result;
//             var deletedUser = (User)okResult.Value!;

//             Assert.Equal(expectedUser.Id, deletedUser.Id);
//             Assert.Equal(expectedUser.FirstName, deletedUser.FirstName);
//             Assert.Equal(expectedUser.LastName, deletedUser.LastName);
//             Assert.Equal(expectedUser.UserName, deletedUser.UserName);

//             uploadPhotosMock.Verify(upload => upload.DeleteFile(expectedUser.PhotoUrl), Times.Once);
//         }

//         [Fact]
//         public async Task DeleteAllUsers_ReturnsOkResult()
//         {
//             // Arrange
//             var userRepositoryMock = new Mock<IUserRepository>();
//             userRepositoryMock.Setup(repo => repo.DeleteAllUsers());

//             var uploadPhotosMock = new Mock<IUploadPhotos>();
//             uploadPhotosMock.Setup(upload => upload.DeleteAllFiles());

//             var controller = new UserController(userRepositoryMock.Object, null!, uploadPhotosMock.Object, null!, null!);

//             // Act
//             var result = await controller.DeleteAllUsers();

//             // Assert
//             Assert.IsType<OkObjectResult>(result);
//             var okResult = (OkObjectResult)result;
//             Assert.Equal("All Users are deleted", okResult.Value!.GetType().GetProperty("message")?.GetValue(okResult.Value));

//             userRepositoryMock.Verify(repo => repo.DeleteAllUsers(), Times.Once);
//             uploadPhotosMock.Verify(upload => upload.DeleteAllFiles(), Times.Once);
//         }

//         [Fact]
//         public async void Login_WithInvalidCredentials_ReturnsUnauthorizedResult()
//         {
//             // Arrange
//             var userRepositoryMock = new Mock<IUserRepository>();
//             userRepositoryMock.Setup(repo => repo.GetUserByUserName("invalid_username"))
//                 .Returns(Task.FromResult((User)null!));

//             var controller = new UserController(userRepositoryMock.Object, null!, null!, null!, null!);

//             var loginData = new Login
//             {
//                 UserName = "invalid_username",
//                 Password = "invalid_password"
//             };

//             // Act
//             var result = await controller.Login(loginData);

//             // Assert
//             var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
//             Assert.Equal("Invalid username or password", unauthorizedResult.Value);

//             userRepositoryMock.Verify(repo => repo.GetUserByUserName("invalid_username"), Times.Once);
//         }


//         [Fact]
//         public async Task SendDetailsToEmail_WithEmailExists_ReturnsOkResult()
//         {
//             // Arrange

//             var user = new User
//             {
//                 Email = "valid_email@example.com",
//                 UserName = "valid_username",
//                 Password = "valid_password"
//             };

//             var userRepositoryMock = new Mock<IUserRepository>();
//             userRepositoryMock.Setup(repo => repo.EmailExists("valid_email@example.com"))
//                 .Returns(Task.FromResult(true));
//             userRepositoryMock.Setup(repo => repo.GetUserByEmail("valid_email@example.com"))
//                 .Returns(Task.FromResult(user));

//             var emailSenderMock = new Mock<IEmailSender>();
//             emailSenderMock.Setup(sender => sender.SendEmail(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
//                 .ReturnsAsync(new Response(HttpStatusCode.OK, null, null));

//             var configMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
//             configMock.Setup(config => config["SendGrid:SubjectForForgotPassword"]).Returns("Registration Subject");
//             configMock.Setup(config => config["SendGrid:ContentForForgotPassword"]).Returns("Registration Content");


//             var controller = new UserController(userRepositoryMock.Object, null!, null!, emailSenderMock.Object, configMock.Object);

//             var emailUser = "valid_email@example.com";

//             // Act
//             var result = await controller.SendDetailsToEmail(emailUser);

//             // Assert
//             var actionResult = Assert.IsType<ActionResult<object>>(result);
//             var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
//             userRepositoryMock.Verify(repo => repo.EmailExists("valid_email@example.com"), Times.Once);
//             userRepositoryMock.Verify(repo => repo.GetUserByEmail("valid_email@example.com"), Times.Once);
//             emailSenderMock.Verify(sender => sender.SendEmail(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
//         }

//         [Fact]
//         public async Task SendDetailsToEmail_WithEmailDoesNotExist_ReturnsBadRequestResult()
//         {
//             // Arrange
//             var userRepositoryMock = new Mock<IUserRepository>();
//             userRepositoryMock.Setup(repo => repo.EmailExists("invalid_email@example.com"))
//                 .Returns(Task.FromResult(false));

//             var controller = new UserController(userRepositoryMock.Object, null!, null!, null!, null!);

//             var emailUser = "invalid_email@example.com";

//             // Act
//             var result = await controller.SendDetailsToEmail(emailUser);

//             // Assert
//             var actionResult = Assert.IsType<ActionResult<object>>(result);
//             var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
//             Assert.Equal("Email invalid_email@example.com does not exist", badRequestResult.Value);

//             userRepositoryMock.Verify(repo => repo.EmailExists("invalid_email@example.com"), Times.Once);
//         }

//         [Fact]
//         public async Task AddUser_WithExistingUsernameAndEmail_ReturnsUnauthorized()
//         {
//             // Arrange
//             var userRepositoryMock = new Mock<IUserRepository>();
//             userRepositoryMock.Setup(repo => repo.UserNameExists(It.IsAny<string>())).Returns(Task.FromResult(true));
//             userRepositoryMock.Setup(repo => repo.EmailExists(It.IsAny<string>())).Returns(Task.FromResult(true));

//             var uploadsMock = new Mock<IUploadPhotos>();
//             uploadsMock.Setup(uploads => uploads.UploadFile(It.IsAny<User>(), It.IsAny<IFormFile>())).ReturnsAsync("uploaded_photo_url");

//             var emailSenderMock = new Mock<IEmailSender>();

//             var configMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
//             configMock.Setup(config => config["SendGrid:SubjectForRegister"]).Returns("Registration Subject");
//             configMock.Setup(config => config["SendGrid:ContentForRegister"]).Returns("Registration Content");

//             var viewModel = new UserViewModel
//             {
//                 UserDTO = new UserDTO
//                 {
//                     FirstName = "John",
//                     LastName = "Doe",
//                     UserName = "existing_username",
//                     Password = "password",
//                     Email = "existing_email@example.com"
//                 },
//                 Photo = Mock.Of<IFormFile>() // Mocking an IFormFile instance
//             };

//             var controller = new UserController(
//                 userRepositoryMock.Object,
//                 null!,
//                 uploadsMock.Object,
//                 emailSenderMock.Object,
//                 configMock.Object
//             );

//             // Act
//             var result = await controller.AddUser(viewModel);

//             // Assert
//             var actionResult = Assert.IsType<ActionResult<User>>(result);
//             var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(actionResult.Result);
//             Assert.Equal("Username existing_username and Email existing_email@example.com already exist", unauthorizedResult.Value);

//             userRepositoryMock.Verify(repo => repo.UserNameExists("existing_username"), Times.Once);
//             userRepositoryMock.Verify(repo => repo.EmailExists("existing_email@example.com"), Times.Once);
//             userRepositoryMock.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Never); // Verify that AddUser method is not called
//             uploadsMock.Verify(uploads => uploads.UploadFile(It.IsAny<User>(), It.IsAny<IFormFile>()), Times.Never); // Verify that UploadFile method is not called
//             emailSenderMock.Verify(sender => sender.SendEmail(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never); // Verify that SendEmailUser method is not called
//         }

//         [Fact]
//         public async Task UpdateUser_WithValidData_ReturnsOkResultWithUpdatedUser()
//         {
//             // Arrange
//             var userRepositoryMock = new Mock<IUserRepository>();

//             var userId = 1;

//             var user = new User
//             {
//                 Id = 1,
//                 UserName = "existing_username",
//                 Email = "existing_email@example.com",
//                 FirstName = "John",
//                 LastName = "Doe",
//                 Password = "existing_password",
//                 PhotoUrl = "existing_photo_url"
//             };

//             userRepositoryMock.Setup(repo => repo.GetUserById(userId))
//                 .Returns(Task.FromResult(user));

//             var uploadsMock = new Mock<IUploadPhotos>();
//             uploadsMock.Setup(uploads => uploads.UpdateFile(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<IFormFile>()))
//                 .ReturnsAsync("updated_photo_url");

//             var emailSenderMock = new Mock<IEmailSender>();
//             var configMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
//             configMock.Setup(config => config["SendGrid:SubjectForUpdate"]).Returns("Update Subject");
//             configMock.Setup(config => config["SendGrid:ContentForUpdate"]).Returns("Update Content");

//             var viewModel = new UpdateUserViewModel
//             {
//                 UpdateUser = new UpdateUserDTO
//                 {
//                     FirstName = "NewFirstName",
//                     LastName = "NewLastName",
//                     Password = "NewPassword"
//                 },
//                 Photo = Mock.Of<IFormFile>() // Mocking an IFormFile instance
//             };

//             var updatedUser = new User
//             {
//                 Id = userId,
//                 UserName = "existing_username",
//                 Email = "existing_email@example.com",
//                 FirstName = "NewFirstName",
//                 LastName = "NewLastName",
//                 Password = "NewPassword",
//                 PhotoUrl = "updated_photo_url"
//             };

//             userRepositoryMock.Setup(repo => repo.UpdateUser(It.IsAny<User>(), It.IsAny<int>())).Returns(Task.FromResult(updatedUser));
//             var controller = new UserController(
//                          userRepositoryMock.Object,
//                          null!,
//                          uploadsMock.Object,
//                          emailSenderMock.Object,
//                          configMock.Object
//                      );

//             // Act
//             var result = await controller.UpdateUser(viewModel, userId);

//             // Assert
//             var okResult = Assert.IsType<OkObjectResult>(result);
//             dynamic responseValue = okResult.Value!;

//             userRepositoryMock.Verify(repo => repo.GetUserById(userId), Times.Once);
//             uploadsMock.Verify(uploads => uploads.UpdateFile(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<IFormFile>()), Times.Once);
//             userRepositoryMock.Verify(repo => repo.UpdateUser(It.IsAny<User>(), It.IsAny<int>()), Times.Once);
//             emailSenderMock.Verify(sender => sender.SendEmail(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
//         }


//         [Fact]
//         public async Task AddUser_WithValidData_ReturnsOk()
//         {
//             // Arrange
//             var userRepositoryMock = new Mock<IUserRepository>();
//             userRepositoryMock.Setup(repo => repo.UserNameExists(It.IsAny<string>())).Returns(Task.FromResult(false));
//             userRepositoryMock.Setup(repo => repo.EmailExists(It.IsAny<string>())).Returns(Task.FromResult(false));

//             var uploadsMock = new Mock<IUploadPhotos>();
//             uploadsMock.Setup(uploads => uploads.UploadFile(It.IsAny<User>(), It.IsAny<IFormFile>())).ReturnsAsync("uploaded_photo_url");

//             var emailSenderMock = new Mock<IEmailSender>();
//             var configMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
//             configMock.Setup(config => config["SendGrid:SubjectForRegister"]).Returns("Registration Subject");
//             configMock.Setup(config => config["SendGrid:ContentForRegister"]).Returns("Registration Content");

//             var viewModel = new UserViewModel
//             {
//                 UserDTO = new UserDTO
//                 {
//                     FirstName = "John",
//                     LastName = "Doe",
//                     UserName = "new_username",
//                     Password = "password",
//                     Email = "new_email@example.com"
//                 },
//                 Photo = Mock.Of<IFormFile>() // Mocking an IFormFile instance
//             };

//             var newUser = new User
//             {
//                 Id = 1,
//                 FirstName = "John",
//                 LastName = "Doe",
//                 UserName = "new_username",
//                 Email = "new_email@example.com"
//                 // Set other properties as needed
//             };

//             userRepositoryMock.Setup(repo => repo.AddUser(It.IsAny<User>())).Returns(Task.FromResult(newUser));
//             var controller = new UserController(
//                                    userRepositoryMock.Object,
//                                    null!,
//                                    uploadsMock.Object,
//                                    emailSenderMock.Object,
//                                    configMock.Object
//                                );

//             // Act
//             var result = await controller.AddUser(viewModel);

//             // Assert
//             var actionResult = Assert.IsType<ActionResult<User>>(result);
//             var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

//             userRepositoryMock.Verify(repo => repo.UserNameExists("new_username"), Times.Once);
//             userRepositoryMock.Verify(repo => repo.EmailExists("new_email@example.com"), Times.Once);
//             userRepositoryMock.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Once);
//             uploadsMock.Verify(uploads => uploads.UploadFile(It.IsAny<User>(), It.IsAny<IFormFile>()), Times.Once);
//             emailSenderMock.Verify(sender => sender.SendEmail(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

//         }
//     }
// }