using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Roles de los usuarios
/// </summary>
public partial class Rol
{
    /// <summary>
    /// Identificador del rol
    /// </summary>
    public int IdRol { get; set; }

    /// <summary>
    /// Nombre del rol (ADMINISTRADOR, COORDINADOR, ASISTENTE)
    /// </summary>
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// Fecha desde la cual el registro es accesible
    /// </summary>
    public DateTime FechaDesde { get; set; }

    /// <summary>
    /// Fecha hasta la cual el registro es accesible
    /// </summary>
    public DateTime? FechaHasta { get; set; }

    /// <summary>
    /// Estado del rol: Activo (A) o Inactivo (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual ICollection<RolUsuarioOrganizacion> RolUsuarioOrganizacion { get; set; } = new List<RolUsuarioOrganizacion>();
}
