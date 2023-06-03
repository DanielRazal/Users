using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Users_Server.Hubs;

namespace Users_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IUserRepository _userRepo;
        private readonly IHubContext<UserHub> _hubContext;


        public MessageController(IMessageRepository messageRepo, IUserRepository userRepo,
        IHubContext<UserHub> hubContext)
        {
            _messageRepo = messageRepo;
            _userRepo = userRepo;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            var messages = await _messageRepo.GetAllMessages();

            return Ok(messages);
        }



        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageDto messageDto, int id)
        {
            var _user = await _userRepo.GetUserById(id);

            if (_user == null)
            {
                return NotFound();
            }

            var message = new Message
            {
                Content = messageDto.Content,
                UserId = _user.Id,
                User = _user
            };

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var messages = await _messageRepo.AddMessage(message);
            var json = JsonSerializer.Serialize(messages, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                IncludeFields = true,
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return Ok(json);
            // await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.User, message.Content);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllMessages()
        {
            await _messageRepo.DeleteAllMessages();
            return Ok(new { message = "All Messages are deleted" });
        }
    }
}