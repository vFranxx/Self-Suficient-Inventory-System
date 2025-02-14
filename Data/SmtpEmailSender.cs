using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace API.Data
{
    public class SmtpEmailSender : IEmailSender, IDisposable
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;

        public SmtpEmailSender(IConfiguration configuration)
        {
            _fromEmail = configuration["FromEmail"];

            _smtpClient = new SmtpClient
            {
                Host = configuration["Smtp:Host"],
                Port = configuration.GetValue<int>("Smtp:Port"),
                Credentials = new NetworkCredential(
                    configuration["Smtp:Username"],
                    configuration["Smtp:Password"]),
                EnableSsl = configuration.GetValue<bool>("Smtp:EnableSsl")
            };
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mailMessage = new MailMessage(_fromEmail, email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }

        public void Dispose()
        {
            _smtpClient?.Dispose();
        }
    }
}
