using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena los países
/// </summary>
public partial class Pais
{
    public int IdPais { get; set; }

    /// <summary>
    /// Nombre del país
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
    /// Estado de la correspondencia: Activa (A) o Inactiva (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual ICollection<Provincia> Provincia { get; set; } = new List<Provincia>();
}
