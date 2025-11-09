using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Usuario1 { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Actividad> Actividad { get; set; } = new List<Actividad>();

    public virtual ICollection<CertificadoParticipacion> CertificadoParticipacion { get; set; } = new List<CertificadoParticipacion>();

    public virtual ICollection<CoordinadorActividad> CoordinadorActividad { get; set; } = new List<CoordinadorActividad>();

    public virtual ICollection<Correspondencia> Correspondencia { get; set; } = new List<Correspondencia>();

    public virtual ICollection<HorarioActividad> HorarioActividad { get; set; } = new List<HorarioActividad>();

    public virtual ICollection<Organizacion> Organizacion { get; set; } = new List<Organizacion>();

    public virtual ICollection<ParticipanteActividad> ParticipanteActividad { get; set; } = new List<ParticipanteActividad>();

    public virtual ICollection<Perfil> Perfil { get; set; } = new List<Perfil>();

    public virtual ICollection<RolUsuarioOrganizacion> RolUsuarioOrganizacionIdUsuarioAdministradorNavigation { get; set; } = new List<RolUsuarioOrganizacion>();

    public virtual ICollection<RolUsuarioOrganizacion> RolUsuarioOrganizacionIdUsuarioAsignadoNavigation { get; set; } = new List<RolUsuarioOrganizacion>();
}
