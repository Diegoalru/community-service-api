using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena el control de los procesos de generación de certificados y envío de correos
/// </summary>
public partial class ControlProcesoGeneracionCer
{
    /// <summary>
    /// Identificador único del control de proceso
    /// </summary>
    public int IdControl { get; set; }

    /// <summary>
    /// Identificador del tipo de control de proceso
    /// </summary>
    public int TipoControl { get; set; }

    /// <summary>
    /// Fecha y hora de inicio de la ejecución del proceso
    /// </summary>
    public DateTime InicioEjecucion { get; set; }

    /// <summary>
    /// Fecha y hora de finalización de la ejecución del proceso
    /// </summary>
    public DateTime FinEjecucion { get; set; }

    /// <summary>
    /// Número de actividades procesadas en el proceso
    /// </summary>
    public int? ActividadesProcesadas { get; set; }

    /// <summary>
    /// Número de certificados generados en el proceso
    /// </summary>
    public int? CertificadosGenerados { get; set; }

    /// <summary>
    /// Número de correos enviados en el proceso
    /// </summary>
    public int? CorreosEnviados { get; set; }

    /// <summary>
    /// Estado del proceso: Espera (S), Completado (C), Error (E), Procesando (P)
    /// </summary>
    public string Estado { get; set; } = null!;

    /// <summary>
    /// Mensaje de error en caso de que el proceso falle
    /// </summary>
    public string? ErrorMensaje { get; set; }

    public virtual TipoControl TipoControlNavigation { get; set; } = null!;
}
