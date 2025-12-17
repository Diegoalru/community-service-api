using System;
using System.Collections.Generic;
using community_service_api.Models.DBTableEntities;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que relaciona los coordinadores con las actividades
/// </summary>
public partial class CoordinadorActividad
{
    /// <summary>
    /// Identificador del registro
    /// </summary>
    public int IdCoordinadorActividad { get; set; }

    /// <summary>
    /// Identificador de la organización
    /// </summary>
    public int IdOrganizacion { get; set; }

    /// <summary>
    /// Identificador de la actividad
    /// </summary>
    public int IdActividad { get; set; }

    /// <summary>
    /// Identificador del usuario coordinador
    /// </summary>
    public int IdUsuarioCoordinador { get; set; }

    /// <summary>
    /// Fecha desde la cual el registro es accesible
    /// </summary>
    public DateTime FechaDesde { get; set; }

    /// <summary>
    /// Fecha hasta la cual el registro es accesible
    /// </summary>
    public DateTime? FechaHasta { get; set; }

    /// <summary>
    /// Estado del registro: Activo (A) o Inactivo (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual Actividad IdActividadNavigation { get; set; } = null!;

    public virtual Organizacion IdOrganizacionNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioCoordinadorNavigation { get; set; } = null!;
}
