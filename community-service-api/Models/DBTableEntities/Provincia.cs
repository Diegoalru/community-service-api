using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena las provincias de cada país
/// </summary>
public partial class Provincia
{
    /// <summary>
    /// Identificador del país al que pertenece la provincia
    /// </summary>
    public int IdPais { get; set; }

    /// <summary>
    /// Identificador único de la provincia dentro del país
    /// </summary>
    public int IdProvincia { get; set; }

    /// <summary>
    /// Código único de la provincia dentro del país
    /// </summary>
    public string Codigo { get; set; } = null!;

    /// <summary>
    /// Nombre de la provincia
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
    /// Estado de la provincia: Activa (A) o Inactiva (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual ICollection<Canton> Canton { get; set; } = new List<Canton>();

    public virtual Pais IdPaisNavigation { get; set; } = null!;
}
