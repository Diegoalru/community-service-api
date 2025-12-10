using community_service_api.MailTemplates;
using community_service_api.Models;
using community_service_api.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace community_service_api.HostedServices;

public class EmailSendService(IServiceScopeFactory serviceScopeFactory, IOptions<EmailSettings> emailSettings)
    : BackgroundService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = serviceScopeFactory.CreateScope();

            var certificacionParticipacionService = scope.ServiceProvider
                .GetRequiredService<ICertificacionParticipacionService>();

            var toNotify = await certificacionParticipacionService.GetCertificateEmailDataAsync();

            foreach (var item in toNotify)
            {
                try
                {
                    if (item.Documento == null || item.Documento.Length == 0)
                    {
                        await certificacionParticipacionService.UpdateSendStatusAsync(item.GetIdCertificacionAsGuid(), false, "PDF document is not available");
                        continue;
                    }

                    await SendEmailAsync(
                        item.EmailVoluntario,
                        CertificateMailTemplate.GetCertificateMailTemplate(),
                        "Certificado Generado y Procesado - Community Service App",
                        item.Documento);

                    await certificacionParticipacionService.UpdateSendStatusAsync(item.GetIdCertificacionAsGuid(), true, null);
                }
                catch (Exception ex)
                {
                    await certificacionParticipacionService.UpdateSendStatusAsync(item.GetIdCertificacionAsGuid(), false, ex.Message);
                }
                finally
                {
                    // Avoid Google detecting Spam by waiting 5 seconds each time.
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
    }

    private async Task SendEmailAsync(string recipientEmail, string mailTemplate, string subject, byte[] pdfAttachment)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Community Service App - Online Services", this._emailSettings.GmailUser));
        message.To.Add(new MailboxAddress("", recipientEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = mailTemplate
        };

        bodyBuilder.Attachments.Add(
            "Certificado_Participacion.pdf",
            pdfAttachment,
            ContentType.Parse("application/pdf"));

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(this._emailSettings.SmtpServer, this._emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(this._emailSettings.GmailUser, this._emailSettings.GmailAppPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
