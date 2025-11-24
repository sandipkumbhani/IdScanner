using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SocPass.Application.Interface;

namespace SocPass.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new ArgumentException("Recipient email is required", nameof(toEmail));

            if (!MailboxAddress.TryParse(toEmail, out var mailboxAddress))
                throw new ArgumentException("Recipient email is not a valid email address.", nameof(toEmail));

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("mail", _configuration["Smtp:From"]));
            message.To.Add(mailboxAddress);
            message.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            message.Body = builder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(
                 _configuration["Smtp:Host"],
                 int.Parse(_configuration["Smtp:Port"]),
                 SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                _configuration["Smtp:Username"],
                _configuration["Smtp:Password"]);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
