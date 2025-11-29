using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace community_service_api.HostedServices;

public class EmailSendService : BackgroundService
{
    private readonly ILogger<EmailSendService> _logger;

    public EmailSendService(ILogger<EmailSendService> logger)
    {
        _logger = logger;
    }

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending pending emails");
            }
        }
    }

    private Task SendPendingEmailsAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // TODO: wire up real email delivery once infrastructure is available
        _logger.LogInformation("Checked and dispatched pending participation emails at {Time}", DateTimeOffset.UtcNow);

        return Task.CompletedTask;
    }
}
