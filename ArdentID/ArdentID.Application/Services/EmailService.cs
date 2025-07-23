using ArdentID.Application.Interfaces.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace ArdentID.Application.Services
{
    public class EmailService(ILogger<EmailService> logger, IConfiguration configuration, IEmailTemplateService templateService) : IEmailService
    {
        private readonly ILogger<EmailService> _logger = logger;
        private readonly IConfiguration _configuration = configuration;
        private readonly IEmailTemplateService _templateService = templateService;

        public async Task SendEmailAsync(string toEmail, string templateKey, Dictionary<string, string> placeholders)
        {
            try
            {
                var subject = await _templateService.GetTemplateSubjectAsync(templateKey);
                var body = await _templateService.GetTemplateHtmlAsync(templateKey, placeholders);

                // The rest of the SMTP logic remains the same...
                var server = _configuration["SmtpSettings:Server"];
                var port = int.Parse(_configuration["SmtpSettings:Port"]!);
                var fromEmail = _configuration["SmtpSettings:FromEmail"];
                var fromName = _configuration["SmtpSettings:FromName"];
                var username = _configuration["SmtpSettings:Username"];
                var password = _configuration["SmtpSettings:Password"];

                using var client = new SmtpClient(server, port) { /* ... */ };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["SmtpSettings:FromEmail"]!, _configuration["SmtpSettings:FromName"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully to {ToEmail} using template {TemplateKey}", toEmail, templateKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send templated email to {ToEmail}", toEmail);
                throw;
            }
        }
    }
}
