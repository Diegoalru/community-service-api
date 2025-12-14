using System;
using System.Collections.Generic;
using community_service_api.Models.DBTableEntities;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena las categorías de las actividades
/// </summary>
public partial class CategoriaActividad
{
    public int IdCategoriaActividad { get; set; }

    /// <summary>
    /// Nombre de la categoría de actividad
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
    /// Estado de la categoría de actividad: Activa (A) o Inactiva (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual ICollection<Actividad> Actividad { get; set; } = new List<Actividad>();
}
