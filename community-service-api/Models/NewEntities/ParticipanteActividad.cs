using System;
using System.Collections.Generic;

namespace community_service_api.Models.NewEntities;

public partial class ParticipanteActividad
{
    public int IdParticipanteActividad { get; set; }

    public int IdOrganizacion { get; set; }

    public int IdActividad { get; set; }

    public int IdHorarioActividad { get; set; }

    public int IdUsuarioVoluntario { get; set; }

    public DateTime FechaInscripcion { get; set; }

    public DateTime? FechaRetiro { get; set; }

    public string Situacion { get; set; } = null!;

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Actividad Actividad { get; set; } = null!;

    public virtual CertificadoParticipacion? CertificadoParticipacion { get; set; }

    public virtual HorarioActividad HorarioActividad { get; set; } = null!;

    public virtual Organizacion IdOrganizacionNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioVoluntarioNavigation { get; set; } = null!;
}
