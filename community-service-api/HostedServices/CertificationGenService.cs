using System.Globalization;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace community_service_api.HostedServices;

public class CertificationGenService : BackgroundService
{
    public CertificationGenService()
    {
        Settings.License = LicenseType.Community;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(20));
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
            catch (Exception)
            {
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

        // TODO: save certificateBytes to database BLOB column instead of disk

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
                page.DefaultTextStyle(t => t.FontFamily(Fonts.Arial));

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
