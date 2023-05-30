using SendGrid;
using SendGrid.Helpers.Mail;

namespace Users_Server.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Response> SendEmail(User user, string subject, string content)
        {
            var sendGridConfig = _configuration.GetSection("SendGrid");
            var emailSender = sendGridConfig.GetValue<string>("EmailSender");
            var senderName = sendGridConfig.GetValue<string>("SenderName");

            var apiKey = Environment.GetEnvironmentVariable("ApiKey");
            if (apiKey == null)
            {
                throw new ArgumentNullException("API key is not found.");
            }
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(emailSender, senderName));
            msg.AddTo(new EmailAddress(user.Email, user.FirstName + user.LastName));
            msg.SetSubject(subject);
            msg.HtmlContent = content;
            var sendEmail = await client.SendEmailAsync(msg);
            return sendEmail;
        }
    }
}
