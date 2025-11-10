using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

public partial class Organizacion
{
    public int IdOrganizacion { get; set; }

    public int IdUsuarioCreador { get; set; }

    public int IdUbicacion { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? SitioWeb { get; set; }

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Actividad> Actividad { get; set; } = new List<Actividad>();

    public virtual ICollection<CoordinadorActividad> CoordinadorActividad { get; set; } = new List<CoordinadorActividad>();

    public virtual ICollection<HorarioActividad> HorarioActividad { get; set; } = new List<HorarioActividad>();

    public virtual Ubicacion IdUbicacionNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioCreadorNavigation { get; set; } = null!;

    public virtual ICollection<ParticipanteActividad> ParticipanteActividad { get; set; } = new List<ParticipanteActividad>();

    public virtual ICollection<RolUsuarioOrganizacion> RolUsuarioOrganizacion { get; set; } = new List<RolUsuarioOrganizacion>();
}
