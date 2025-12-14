using System;
using System.Collections.Generic;
using community_service_api.Models.DBTableEntities;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena las organizaciones registradas en el sistema
/// </summary>
public partial class Organizacion
{
    /// <summary>
    /// Identificador único de la organización
    /// </summary>
    public int IdOrganizacion { get; set; }

    /// <summary>
    /// Identificador del usuario que creó la organización
    /// </summary>
    public int IdUsuarioCreador { get; set; }

    /// <summary>
    /// Identificador de la ubicación de la organización
    /// </summary>
    public int IdUbicacion { get; set; }

    public int? IdUniversidad { get; set; }

    /// <summary>
    /// Nombre de la organización
    /// </summary>
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// Descripción de la organización
    /// </summary>
    public string Descripcion { get; set; } = null!;

    /// <summary>
    /// Fecha desde la cual el registro es accesible
    /// </summary>
    public DateTime FechaDesde { get; set; }

    /// <summary>
    /// Fecha hasta la cual el registro es accesible
    /// </summary>
    public DateTime? FechaHasta { get; set; }

    /// <summary>
    /// Estado de la organización: Activo (A) o Inactivo (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual ICollection<Actividad> Actividad { get; set; } = new List<Actividad>();

    public virtual ICollection<CoordinadorActividad> CoordinadorActividad { get; set; } = new List<CoordinadorActividad>();

    public virtual ICollection<Correspondencia> Correspondencia { get; set; } = new List<Correspondencia>();

    public virtual ICollection<HorarioActividad> HorarioActividad { get; set; } = new List<HorarioActividad>();

    public virtual Ubicacion IdUbicacionNavigation { get; set; } = null!;

    public virtual Universidad? IdUniversidadNavigation { get; set; }

    public virtual Usuario IdUsuarioCreadorNavigation { get; set; } = null!;

    public virtual ICollection<ParticipanteActividad> ParticipanteActividad { get; set; } = new List<ParticipanteActividad>();

    public virtual ICollection<RolUsuarioOrganizacion> RolUsuarioOrganizacion { get; set; } = new List<RolUsuarioOrganizacion>();
}
