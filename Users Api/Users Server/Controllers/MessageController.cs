using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Users_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IUserRepository _userRepo;


        public MessageController(IMessageRepository messageRepo, IUserRepository userRepo)
        {
            _messageRepo = messageRepo;
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllMessages()
        {
            var messages = await _messageRepo.GetAllMessages();

            return Ok(messages);
        }



        [HttpPost]
        public async Task<ActionResult<Message>> AddMessage(MessageDto messageDto, int id)
        {
            var _user = await _userRepo.GetUserById(id);

            if (_user == null)
            {
                return Unauthorized("Not Authorized");
            }

            var message = new Message
            {
                Content = messageDto.Content,
                UserId = _user.Id,
                User = _user
            };

            if (message == null)
            {
                return BadRequest();
            }

            var messages = await _messageRepo.AddMessage(message);

            return Ok(messages);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllMessages()
        {
            await _messageRepo.DeleteAllMessages();
            return Ok(new { message = "All Messages are deleted" });
        }
    }
}