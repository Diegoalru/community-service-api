using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena los tipos de control de procesos
/// </summary>
public partial class TipoControl
{
    /// <summary>
    /// Identificador único del tipo de control de proceso
    /// </summary>
    public int IdTipoControl { get; set; }

    /// <summary>
    /// Nombre del tipo de control de proceso (e.g., Generación de Certificados, envío de correos)
    /// </summary>
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// Estado del tipo de control de proceso: Activo (A) o Inactivo (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual ICollection<ControlProcesoGeneracionCer> ControlProcesoGeneracionCer { get; set; } = new List<ControlProcesoGeneracionCer>();
}
