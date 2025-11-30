using community_service_api.Models.DBTableEntities;
using community_service_api.Services;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace community_service_api.HostedServices;

public class CertificationGenService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CertificationGenService(IServiceScopeFactory serviceScopeFactory)
    {
        Settings.License = LicenseType.Community;
        _serviceScopeFactory = serviceScopeFactory;
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

    private async Task GenerateSampleCertificateAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // TODO: Get pending records from Db here!

        var certificado = new CertificadoParticipacion
        {
            IdCertificacion = 1,
            IdParticipanteActividad = 1001,
            IdActividad = 2001,
            IdOrganizacion = 3001,
            IdUsuarioVoluntario = 4001,
            FechaEmision = DateTime.UtcNow,
            HorasTotales = 40,
            DiasTotales = 10,
            FechaPrimeraAsistencia = DateTime.UtcNow.AddMonths(-2),
            FechaUltimaAsistencia = DateTime.UtcNow.AddDays(-7),
            Observaciones = "Gracias por tu dedicación y compromiso continuo.",
            Situacion = "E",
            Estado = "A",
        };

        var participantName = "Community Participant";
        var organizationName = "Community Service Organization";
        var activityName = "Community Service Activity";

        var certificateBytes = BuildCertificatePdf(certificado, participantName, organizationName, activityName);

        // Example of persisting to disk (you could also persist the byte[] directly to a database BLOB column)
        var certificatesDirectory = Path.Combine(AppContext.BaseDirectory, "certificates");
        Directory.CreateDirectory(certificatesDirectory);

        var fileName = $"certificate_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf";
        var filePath = Path.Combine(certificatesDirectory, fileName);
        File.WriteAllBytes(filePath, certificateBytes);

        using var scope = _serviceScopeFactory.CreateScope();
        var certificacionParticipacionService = scope.ServiceProvider
            .GetRequiredService<ICertificacionParticipacionService>();

        await certificacionParticipacionService.SaveCertificateDocumentAsync(
            certificado.IdCertificacion,
            certificateBytes,
            cancellationToken);
    }

    private static byte[] BuildCertificatePdf(
        CertificadoParticipacion certificado,
        string participantName,
        string organizationName,
        string activityName)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter.Landscape());
                page.Margin(1.5f, Unit.Centimetre);
                page.PageColor(Colors.Grey.Lighten4);
                page.DefaultTextStyle(t => t.FontFamily(Fonts.Calibri).FontSize(12));

                page.Content().Padding(1.5f, Unit.Centimetre).Decoration(decoration =>
                {
                    decoration.Before().Border(4).BorderColor(Colors.Brown.Medium);

                    decoration.Content().Padding(1.5f, Unit.Centimetre).Border(1).BorderColor(Colors.Grey.Medium)
                        .Background(Colors.White)
                        .Column(column =>
                        {
                            column.Spacing(12);

                            column.Item().Text(organizationName)
                                .FontSize(16)
                                .LetterSpacing(2)
                                .SemiBold()
                                .AlignCenter()
                                .FontColor(Colors.Brown.Medium);

                            column.Item().Text("Certificado de Participación")
                                .FontSize(34)
                                .SemiBold()
                                .AlignCenter();

                            column.Item().Text(text =>
                            {
                                text.Span("Este certificado se otorga a ").FontSize(14);
                                text.Span(participantName).FontSize(22).SemiBold();
                            });

                            column.Item().Text($"Por su valiosa contribución a la actividad: {activityName}")
                                .FontSize(16)
                                .AlignCenter();

                            column.Item().PaddingVertical(10).Row(row =>
                            {
                                row.RelativeItem().Column(stack =>
                                {
                                    stack.Item().Text("Detalles de participación").FontSize(14).SemiBold();
                                    stack.Item().PaddingTop(4).Text(text =>
                                    {
                                        text.Span("Horas totales: ").SemiBold();
                                        text.Span(certificado.HorasTotales.ToString(CultureInfo.InvariantCulture));
                                        text.Line("Días totales: " + certificado.DiasTotales.ToString(CultureInfo.InvariantCulture));
                                        text.Line(
                                            $"Primera asistencia: {certificado.FechaPrimeraAsistencia:dd 'de' MMMM yyyy}");
                                        text.Line(
                                            $"Última asistencia: {certificado.FechaUltimaAsistencia:dd 'de' MMMM yyyy}");
                                    });
                                });

                                row.ConstantItem(1, Unit.Centimetre);

                                row.RelativeItem().Column(stack =>
                                {
                                    stack.Item().Text("Datos del certificado").FontSize(14).SemiBold();
                                    stack.Item().PaddingTop(4).Text(text =>
                                    {
                                        text.Line($"ID Certificación: {certificado.IdCertificacion}");
                                        text.Line($"Emitido el: {certificado.FechaEmision:dd 'de' MMMM yyyy}");
                                        text.Line($"Situación: {certificado.Situacion}");
                                        text.Line($"Estado: {certificado.Estado}");
                                    });
                                });
                            });

                            if (!string.IsNullOrWhiteSpace(certificado.Observaciones))
                            {
                                column.Item().PaddingTop(6).Text(text =>
                                {
                                    text.Span("Observaciones: ").SemiBold();
                                    text.Span(certificado.Observaciones);
                                });
                            }

                            column.Item().PaddingTop(16).Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text("_____________________________").AlignCenter();
                                    col.Item().Text("Representante de la organización").AlignCenter();
                                });

                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text("_____________________________").AlignCenter();
                                    col.Item().Text(participantName).AlignCenter();
                                });
                            });
                        });
                });
            });
        }).GeneratePdf();
    }
}
