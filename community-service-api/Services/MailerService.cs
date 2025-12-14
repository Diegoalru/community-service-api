using community_service_api.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace community_service_api.Services;

public class MailerService(IOptions<EmailSettings> emailSettings) : IMailerService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    public async Task SendEmailAsync(string recipientEmail, string subject, string body, byte[]? attachment = null, string? attachmentName = null)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Community Service App - Online Services", _emailSettings.GmailUser));
        message.To.Add(new MailboxAddress("", recipientEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body
        };

        if (attachment != null && !string.IsNullOrEmpty(attachmentName))
        {
            bodyBuilder.Attachments.Add(attachmentName, attachment, ContentType.Parse("application/pdf"));
        }

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_emailSettings.GmailUser, _emailSettings.GmailAppPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
