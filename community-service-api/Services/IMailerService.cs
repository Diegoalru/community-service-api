namespace community_service_api.Services;

public interface IMailerService
{
    Task SendEmailAsync(string recipientEmail, string subject, string body, byte[]? attachment = null, string? attachmentName = null);
}
