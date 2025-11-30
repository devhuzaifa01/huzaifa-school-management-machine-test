using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using School.Application.Contracts.Services;
using School.Application.Dtos.Configuration;

namespace School.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _emailSettings;

        public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> emailSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSettings = emailSettings?.Value ?? throw new ArgumentNullException(nameof(emailSettings));
        }

        public async Task SendHtmlEmailAsync(string recipientEmail, string subject, string htmlBody)
        {
            try
            {
                _logger.LogInformation($"Sending HTML email to: {recipientEmail}, Subject: {subject}");

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("School Management System", _emailSettings.SenderEmail));
                message.To.Add(new MailboxAddress("", recipientEmail));
                message.Subject = subject;

                message.Body = new TextPart("html")
                {
                    Text = htmlBody
                };

                using var client = new SmtpClient();
                _logger.LogInformation($"Connecting to SMTP Server");
                var secureSocketOptions = _emailSettings.UseSSL ? MailKit.Security.SecureSocketOptions.SslOnConnect : MailKit.Security.SecureSocketOptions.StartTls;
                await client.ConnectAsync(_emailSettings.SMTPServer, _emailSettings.SMTPPort, secureSocketOptions);

                _logger.LogInformation($"Authenticating to Sender Email and Sender Password");
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);

                _logger.LogInformation($"Sending HTML email to: {recipientEmail}");
                await client.SendAsync(message);

                _logger.LogInformation($"Disconnecting from SMTP Server");
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in SendHtmlEmailAsync for email: {recipientEmail}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task SendHtmlEmailAsync(List<string> recipientEmails, string subject, string htmlBody)
        {
            try
            {
                if (recipientEmails == null || recipientEmails.Count == 0)
                {
                    throw new ArgumentException("Recipient emails list cannot be null or empty", nameof(recipientEmails));
                }

                _logger.LogInformation($"Sending HTML email to {recipientEmails.Count} recipients, Subject: {subject}");

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("School Management System", _emailSettings.SenderEmail));
                
                foreach (var email in recipientEmails)
                {
                    message.To.Add(new MailboxAddress("", email));
                }

                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = htmlBody
                };

                using var client = new SmtpClient();
                _logger.LogInformation($"Connecting to SMTP Server");
                var secureSocketOptions = _emailSettings.UseSSL ? MailKit.Security.SecureSocketOptions.SslOnConnect : MailKit.Security.SecureSocketOptions.StartTls;
                await client.ConnectAsync(_emailSettings.SMTPServer, _emailSettings.SMTPPort, secureSocketOptions);

                _logger.LogInformation($"Authenticating to Sender Email and Sender Password");
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);

                _logger.LogInformation($"Sending HTML email to {recipientEmails.Count} recipients");
                await client.SendAsync(message);

                _logger.LogInformation($"Disconnecting from SMTP Server");
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in SendHtmlEmailAsync for multiple recipients. {ex.Message}", ex);
                throw;
            }
        }
    }
}

