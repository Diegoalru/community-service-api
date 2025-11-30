namespace community_service_api.HostedServices;

public class EmailSendService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(10));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await SendPendingEmailsAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception)
            {
            }
        }
    }

    private Task SendPendingEmailsAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // TODO: Set up gmail app secret to send emails + SMTP client

        return Task.CompletedTask;
    }
}
