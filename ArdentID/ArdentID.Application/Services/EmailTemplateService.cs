using ArdentID.Application.Interfaces.Shared;
using Microsoft.Extensions.Hosting; // Required for IHostEnvironment
using System.Text.Json;

// The namespace should now reflect its new location in the Infrastructure project
namespace ArdentID.Application.Services
{
    /// <summary>
    /// Reads and processes email templates from a JSON file.
    /// </summary>
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly Dictionary<string, JsonElement> _templates;

        /// <summary>
        /// Initializes the service by loading email templates.
        /// </summary>
        /// <param name="environment">The hosting environment, injected by DI to find the application's root path.</param>
        public EmailTemplateService(IHostEnvironment environment)
        {
            // Construct an absolute and reliable path to the template file.
            // environment.ContentRootPath points to the root of the running application (ArdentID.Presentation).
            var filePath = Path.Combine(environment.ContentRootPath, "email-templates.json");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(
                    "The 'email-templates.json' file could not be found. " +
                    "Ensure the file is placed in the root of the 'ArdentID.Presentation' project and its 'Copy to Output Directory' property is set to 'Copy if newer'.",
                    filePath);
            }

            var jsonText = File.ReadAllText(filePath);
            _templates = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonText)!;
        }

        public Task<string> GetTemplateHtmlAsync(string templateKey, Dictionary<string, string> placeholders)
        {
            if (!_templates.TryGetValue(templateKey, out var templateElement))
            {
                throw new KeyNotFoundException($"Email template with key '{templateKey}' not found.");
            }

            var htmlBody = templateElement.GetProperty("Body").GetString()!;
            foreach (var placeholder in placeholders)
            {
                htmlBody = htmlBody.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }
            return Task.FromResult(htmlBody);
        }

        public Task<string> GetTemplateSubjectAsync(string templateKey)
        {
            if (!_templates.TryGetValue(templateKey, out var templateElement))
            {
                throw new KeyNotFoundException($"Email template with key '{templateKey}' not found.");
            }
            return Task.FromResult(templateElement.GetProperty("Subject").GetString()!);
        }
    }
}
