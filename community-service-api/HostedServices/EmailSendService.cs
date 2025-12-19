using community_service_api.MailTemplates;
using community_service_api.Services;

namespace community_service_api.HostedServices;

public class EmailSendService(IServiceScopeFactory serviceScopeFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(3));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = serviceScopeFactory.CreateScope();
            var mailerService = scope.ServiceProvider.GetRequiredService<IMailerService>();
            var emailQueueService = scope.ServiceProvider.GetRequiredService<IEmailQueueService>();

            var pendingEmails = await emailQueueService.GetPendingEmailsAsync();

            foreach (var email in pendingEmails)
            {
                try
                {
                    await mailerService.SendEmailAsync(
                        email.Destinatario,
                        email.Asunto,
                        email.Cuerpo,
                        email.Adjunto,
                        email.NombreAdjunto);

                    await emailQueueService.UpdateEmailStatusAsync(email.IdCorreoPendiente, true);
                }
                catch (Exception ex)
                {
                    await emailQueueService.UpdateEmailStatusAsync(email.IdCorreoPendiente, false, ex.Message);
                }
                finally
                {
                    // Avoid Google detecting Spam by waiting 5 seconds each time.
                    // This delay applies per email sent by this background service.
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
    }
}
