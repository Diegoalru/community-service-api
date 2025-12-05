using community_service_api.MailTemplates;
using community_service_api.Models;
using community_service_api.Models.Dtos;
using community_service_api.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace community_service_api.HostedServices;

public class EmailSendService : BackgroundService
{
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly EmailSettings emailSettings;

    public EmailSendService(IServiceScopeFactory serviceScopeFactory, IOptions<EmailSettings> emailSettings)
    {
        this.serviceScopeFactory = serviceScopeFactory;
        this.emailSettings = emailSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            var toNotify = await this.GetCertificates();

            foreach (var item in toNotify)
            {
                try
                {
                    await SendEmailAsync(
                        item.EmailVoluntario,
                        CertificateMailTemplate.GetCertificateMailTemplate(),
                        "Certificado Generado y Procesado - Community Service App",
                        item.Documento);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    // Avoid Google detecting Spam by waiting 5 seconds each time.
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
    }

    public async Task SendEmailAsync(string recipientEmail, string mailTemplate, string subject, byte[] pdfAttachment)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Community Service App - Online Services", this.emailSettings.GmailUser));
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
        await client.ConnectAsync(this.emailSettings.SmtpServer, this.emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(this.emailSettings.GmailUser, this.emailSettings.GmailAppPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task<IEnumerable<CertificatePdfDataDto>> GetCertificates() 
    {
        using var scope = this.serviceScopeFactory.CreateScope();

        var certificacionParticipacionService = scope.ServiceProvider
            .GetRequiredService<ICertificacionParticipacionService>();

        return await certificacionParticipacionService.GetCertificatePdfDataAsync("G");   
    }
}
