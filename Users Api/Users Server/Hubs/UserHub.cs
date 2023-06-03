using Microsoft.AspNetCore.SignalR;

namespace Users_Server.Hubs
{
    public class UserHub : Hub
    {
        public async Task SendMessage(User user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
