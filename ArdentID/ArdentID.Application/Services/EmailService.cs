using ArdentID.Application.Interfaces.Shared;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace ArdentID.Application.Services
{
    /// <summary>
    /// Implements email sending functionality using the modern MailKit library.
    /// </summary>
    public class EmailService(ILogger<EmailService> logger, IConfiguration configuration, IEmailTemplateService templateService) : IEmailService
    {
        private readonly ILogger<EmailService> _logger = logger;
        private readonly IConfiguration _configuration = configuration;
        private readonly IEmailTemplateService _templateService = templateService;

        public async Task SendEmailAsync(string toEmail, string templateKey, Dictionary<string, string> placeholders)
        {
            try
            {
                // 1. Get the email subject and HTML body from the template service.
                var subject = await _templateService.GetTemplateSubjectAsync(templateKey);
                var htmlBody = await _templateService.GetTemplateHtmlAsync(templateKey, placeholders);

                // 2. Get SMTP settings from appsettings.json.
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var server = smtpSettings["Server"];
                var port = int.Parse(smtpSettings["Port"]!);
                var fromName = smtpSettings["FromName"];
                var fromEmail = smtpSettings["FromEmail"];
                var username = smtpSettings["Username"];
                var password = smtpSettings["Password"];

                // 3. Create the email message using MimeKit.
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;

                // 4. Set the body of the email.
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlBody
                };
                message.Body = bodyBuilder.ToMessageBody();

                // 5. Use the MailKit SmtpClient to connect and send.
                using var client = new MailKit.Net.Smtp.SmtpClient();

                // Connect to the server using STARTTLS.
                // This will connect on the specified port and then upgrade to a secure connection.
                await client.ConnectAsync(server, port, SecureSocketOptions.StartTls);

                // Authenticate with your username and App Password.
                await client.AuthenticateAsync(username, password);

                // Send the email.
                await client.SendAsync(message);

                // Disconnect gracefully.
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully to {ToEmail} using template {TemplateKey}", toEmail, templateKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send templated email to {ToEmail} using MailKit.", toEmail);
                // Re-throw the exception so the calling layer can handle it.
                throw;
            }
        }
    }
}
