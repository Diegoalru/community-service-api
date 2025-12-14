using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que relaciona los usuarios con sus organizaciones y sus roles
/// </summary>
public partial class RolUsuarioOrganizacion
{
    /// <summary>
    /// Identificador del registro
    /// </summary>
    public int IdRolUsuarioOrganizacion { get; set; }

    /// <summary>
    /// Identificador de la organización
    /// </summary>
    public int IdOrganizacion { get; set; }

    /// <summary>
    /// Identificador del usuario asignado
    /// </summary>
    public int IdUsuarioAsignado { get; set; }

    /// <summary>
    /// Identificador del usuario administrador
    /// </summary>
    public int IdUsuarioAdministrador { get; set; }

    /// <summary>
    /// Identificador del rol
    /// </summary>
    public int IdRol { get; set; }

    /// <summary>
    /// Fecha desde la cual el registro es accesible
    /// </summary>
    public DateTime FechaDesde { get; set; }

    /// <summary>
    /// Fecha hasta la cual el registro es accesible
    /// </summary>
    public DateTime? FechaHasta { get; set; }

    /// <summary>
    /// Estado del registro: Activo (A) o Inactivo (I) para borrado lógico.
    /// </summary>
    public string Estado { get; set; } = null!;

    /// <summary>
    /// Indica si la asignación del rol está activa (A) o inactiva (I).
    /// </summary>
    public string EsActivo { get; set; } = null!;

    public virtual Organizacion IdOrganizacionNavigation { get; set; } = null!;

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioAdministradorNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioAsignadoNavigation { get; set; } = null!;
}
