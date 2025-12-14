using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena los distritos de cada cantón
/// </summary>
public partial class Distrito
{
    /// <summary>
    /// Identificador del país al que pertenece el distrito
    /// </summary>
    public int IdPais { get; set; }

    /// <summary>
    /// Identificador de la provincia a la que pertenece el distrito
    /// </summary>
    public int IdProvincia { get; set; }

    /// <summary>
    /// Identificador del cantón al que pertenece el distrito
    /// </summary>
    public int IdCanton { get; set; }

    /// <summary>
    /// Identificador único del distrito dentro del cantón
    /// </summary>
    public int IdDistrito { get; set; }

    /// <summary>
    /// Código único del distrito dentro del cantón
    /// </summary>
    public string Codigo { get; set; } = null!;

    /// <summary>
    /// Nombre del distrito
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
    /// Estado del distrito: Activa (A) o Inactiva (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual Canton Canton { get; set; } = null!;

    public virtual ICollection<Ubicacion> Ubicacion { get; set; } = new List<Ubicacion>();
}
