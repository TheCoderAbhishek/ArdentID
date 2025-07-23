namespace ArdentID.Application.Interfaces.Shared
{
    public interface IEmailTemplateService
    {
        Task<string> GetTemplateHtmlAsync(string templateKey, Dictionary<string, string> placeholders);
        Task<string> GetTemplateSubjectAsync(string templateKey);
    }
}
