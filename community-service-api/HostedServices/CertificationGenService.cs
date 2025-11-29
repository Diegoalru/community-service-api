using System.Globalization;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace community_service_api.HostedServices;

public class CertificationGenService : BackgroundService
{
    private readonly ILogger<CertificationGenService> _logger;

    public CertificationGenService(ILogger<CertificationGenService> logger)
    {
        _logger = logger;
        Settings.License = LicenseType.Community;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(10));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await GenerateSampleCertificateAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Cancellation requested, exit gracefully
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating participation certificate");
            }
        }
    }

    private Task GenerateSampleCertificateAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var participantName = "Community Participant";
        var activityName = "Community Service Activity";
        var participationDate = DateTime.UtcNow.ToString("D", CultureInfo.InvariantCulture);

        var certificateBytes = BuildCertificatePdf(participantName, activityName, participationDate);

        // Example of persisting to disk (you could also persist the byte[] directly to a database BLOB column)
        var certificatesDirectory = Path.Combine(AppContext.BaseDirectory, "certificates");
        Directory.CreateDirectory(certificatesDirectory);

        var fileName = $"certificate_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf";
        var filePath = Path.Combine(certificatesDirectory, fileName);
        File.WriteAllBytes(filePath, certificateBytes);

        _logger.LogInformation("Generated participation certificate at {FilePath} (size: {ByteCount} bytes)", filePath, certificateBytes.Length);

        // If you prefer a stream to send to Oracle, wrap the bytes in a MemoryStream and dispose after use.
        using var certificateStream = new MemoryStream(certificateBytes);
        _ = certificateStream.Length; // placeholder to illustrate availability for further processing

        return Task.CompletedTask;
    }

    private static byte[] BuildCertificatePdf(string participantName, string activityName, string participationDate)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(t => t.FontFamily(Fonts.Sans));

                page.Header().Row(row =>
                {
                    row.RelativeItem().Text("Participation Certificate")
                        .FontSize(28)
                        .SemiBold()
                        .AlignCenter();
                });

                page.Content().PaddingVertical(2, Unit.Centimetre).Column(column =>
                {
                    column.Item().Text("This certificate is proudly presented to").FontSize(14).AlignCenter();
                    column.Item().Text(participantName).FontSize(24).Bold().AlignCenter();
                    column.Item().PaddingTop(10).Text($"For participating in: {activityName}").FontSize(16).AlignCenter();
                    column.Item().Text($"Date: {participationDate}").FontSize(12).AlignCenter();
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Generated on ").FontSize(10);
                    text.Span(DateTime.UtcNow.ToString("f", CultureInfo.InvariantCulture)).FontSize(10).Bold();
                });
            });
        }).GeneratePdf();
    }
}
