namespace ArdentID.Application.Interfaces.Shared
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string templateKey, Dictionary<string, string> placeholders);
    }
}
