using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

public partial class Actividad
{
    public int IdActividad { get; set; }

    public int IdOrganizacion { get; set; }

    public int IdUsuarioCreador { get; set; }

    public int IdCategoria { get; set; }

    public int IdUbicacion { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int? Horas { get; set; }

    public int Cupos { get; set; }

    public string Situacion { get; set; } = null!;

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<CertificadoParticipacion> CertificadoParticipacion { get; set; } = new List<CertificadoParticipacion>();

    public virtual ICollection<CoordinadorActividad> CoordinadorActividad { get; set; } = new List<CoordinadorActividad>();

    public virtual ICollection<HorarioActividad> HorarioActividad { get; set; } = new List<HorarioActividad>();

    public virtual CategoriaActividad IdCategoriaNavigation { get; set; } = null!;

    public virtual Organizacion IdOrganizacionNavigation { get; set; } = null!;

    public virtual Ubicacion IdUbicacionNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioCreadorNavigation { get; set; } = null!;

    public virtual ICollection<ParticipanteActividad> ParticipanteActividad { get; set; } = new List<ParticipanteActividad>();
}
