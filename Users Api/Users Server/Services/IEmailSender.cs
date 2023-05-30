using SendGrid;

namespace Users_Server.Services
{
    public interface IEmailSender
    {
        Task<Response> SendEmail(User user, string subject, string content);
    }
}
