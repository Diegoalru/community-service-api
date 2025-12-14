using System.Globalization;
using community_service_api.Models.Dtos;
using community_service_api.Services;
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
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await GenerateSampleCertificateAsync(stoppingToken);
            }
            catch (OperationCanceledException ex)
            {
                break;
            }
            catch (Exception ex)
            {
                // ignored
            }
        }
    }

    private async Task GenerateSampleCertificateAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var scope = _serviceScopeFactory.CreateScope();
        var certificacionParticipacionService = scope.ServiceProvider
            .GetRequiredService<ICertificacionParticipacionService>();
        var emailQueueService = scope.ServiceProvider.GetRequiredService<IEmailQueueService>();

        var data = await certificacionParticipacionService.GetCertificateGenerationDataAsync();

        foreach (var item in data)
        {
            var participantName = item.NombreVoluntario;
            var organizationName = item.NombreOrganizacion;
            var activityName = item.NombreActividad;

            var certificateBytes = BuildCertificatePdf(item, participantName, organizationName, activityName);

            await certificacionParticipacionService.SaveCertificateDocumentAsync(
                item.GetIdCertificacionAsGuid(),
                certificateBytes,
                cancellationToken);

            if (!string.IsNullOrEmpty(item.EmailVoluntario))
            {
                await emailQueueService.EnqueueEmailAsync(
                    item.EmailVoluntario,
                    "Certificado Generado y Procesado - Community Service App",
                    community_service_api.MailTemplates.CertificateMailTemplate.GetCertificateMailTemplate(),
                    idUsuario: item.IdUsuarioVoluntario,
                    attachment: certificateBytes,
                    attachmentName: "Certificado_Participacion.pdf",
                    idCertificacion: item.GetIdCertificacionAsGuid());
            }
        }
    }

    private static byte[] BuildCertificatePdf(
        CertificateGenerationDataDto certificado,
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
                    decoration.Before().Border(4).BorderColor(Colors.BlueGrey.Medium);

                    decoration.Content().Padding(1.5f, Unit.Centimetre).Border(1).BorderColor(Colors.Grey.Medium)
                        .Background(Colors.White)
                        .Padding(1f, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(12);

                            column.Item().Text(organizationName)
                                .FontSize(16)
                                .SemiBold()
                                .AlignCenter()
                                .FontColor(Colors.Teal.Medium);

                            column.Item().Text("Certificado de Participación")
                                .FontSize(34)
                                .SemiBold()
                                .AlignCenter();

                            column.Item().Text(text =>
                            {
                                text.Span("Este certificado se otorga a ").FontSize(14);
                                text.Span(participantName).FontSize(22).SemiBold();

                                text.AlignCenter();
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
                                        text.Line(" Días totales: " + certificado.DiasTotales.ToString(CultureInfo.InvariantCulture));
                                        text.Line(
                                            $"Primera asistencia: {certificado.FechaInicio:dd 'de' MMMM yyyy}");
                                        text.Line(
                                            $"Última asistencia: {certificado.FechaFin:dd 'de' MMMM yyyy}");
                                    });
                                });

                                row.ConstantItem(1, Unit.Centimetre);

                                row.RelativeItem().Column(stack =>
                                {
                                    stack.Item().Text("Datos del certificado").FontSize(14).SemiBold();
                                    stack.Item().PaddingTop(4).Text(text =>
                                    {
                                        text.Line($"ID Certificación: {(certificado.IdCertificacion is { Length: 16 } raw16 ? new Guid(raw16).ToString() : BitConverter.ToString(certificado.IdCertificacion))}");
                                        text.Line($"Emitido el: {certificado.FechaEmisionTexto:dd 'de' MMMM yyyy}");
                                        text.Line($"Lugar: {certificado.LugarEvento}");
                                    });
                                });
                            });
                        });
                });
            });
        }).GeneratePdf();
    }
}