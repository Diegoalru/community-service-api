using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tipos de corresponsalidad
/// </summary>
public partial class TipoCorrespondencia
{
    public int IdTipoCorrespondencia { get; set; }

    /// <summary>
    /// Descripción del tipo de corresponsalidad (e.g., Email, Celular, Oficina)
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
    /// Estado del tipo de corresponsalidad: Activo (A) o Inactivo (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual ICollection<Correspondencia> Correspondencia { get; set; } = new List<Correspondencia>();
}
