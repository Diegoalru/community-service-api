using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena los cantones de cada provincia
/// </summary>
public partial class Canton
{
    /// <summary>
    /// Identificador del país al que pertenece el cantón
    /// </summary>
    public int IdPais { get; set; }

    /// <summary>
    /// Identificador de la provincia a la que pertenece el cantón
    /// </summary>
    public int IdProvincia { get; set; }

    /// <summary>
    /// Identificador único del cantón dentro de la provincia
    /// </summary>
    public int IdCanton { get; set; }

    /// <summary>
    /// Código único del cantón dentro de la provincia
    /// </summary>
    public string Codigo { get; set; } = null!;

    /// <summary>
    /// Nombre del cantón
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
    /// Estado del cantón: Activa (A) o Inactiva (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual ICollection<Distrito> Distrito { get; set; } = new List<Distrito>();

    public virtual Provincia Provincia { get; set; } = null!;
}
