using System;
using System.Collections.Generic;

namespace community_service_api.Models.NewEntities;

public partial class CertificadoParticipacion
{
    public Guid IdCertificacion { get; set; }

    public int IdParticipanteActividad { get; set; }

    public int IdActividad { get; set; }

    public int IdOrganizacion { get; set; }

    public int IdUsuarioVoluntario { get; set; }

    public DateTime FechaEmision { get; set; }

    public int HorasTotales { get; set; }

    public int DiasTotales { get; set; }

    public DateTime FechaPrimeraAsistencia { get; set; }

    public DateTime FechaUltimaAsistencia { get; set; }

    public string Situacion { get; set; } = null!;

    public string? Observaciones { get; set; }

    public byte[]? Documento { get; set; }

    public DateTime? FechaEnvio { get; set; }

    public int? IntentosEnvio { get; set; }

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Actividad Actividad { get; set; } = null!;

    public virtual Usuario IdUsuarioVoluntarioNavigation { get; set; } = null!;

    public virtual ParticipanteActividad ParticipanteActividad { get; set; } = null!;
}
