using CareerSpotlightApi.Models.Settings;
using CareerSpotlightApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CareerSpotlightApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public void SendEmail(string to, string subject, string body)
        {
            var from = _emailSettings.From;
            var host = _emailSettings.Host;
            var port = _emailSettings.Port;
            var username = _emailSettings.Username;
            var password = _emailSettings.Password;

            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(from ?? string.Empty, to, subject, body);
            client.Send(mailMessage);
        }
    }
}
