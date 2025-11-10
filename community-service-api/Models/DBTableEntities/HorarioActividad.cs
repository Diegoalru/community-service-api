using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

public partial class HorarioActividad
{
    public int IdHorarioActividad { get; set; }

    public int IdOrganizacion { get; set; }

    public int IdActividad { get; set; }

    public int IdUsuario { get; set; }

    public DateTime Fecha { get; set; }

    public DateTime HoraInicio { get; set; }

    public DateTime HoraFin { get; set; }

    public string? Descripcion { get; set; }

    public string Situacion { get; set; } = null!;

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Actividad Actividad { get; set; } = null!;

    public virtual Organizacion IdOrganizacionNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<ParticipanteActividad> ParticipanteActividad { get; set; } = new List<ParticipanteActividad>();
}
