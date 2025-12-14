using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena los errores producidos en el sistema
/// </summary>
public partial class LogError
{
    /// <summary>
    /// Identificador único del error registrado
    /// </summary>
    public int IdLogError { get; set; }

    /// <summary>
    /// Usuario que experimentó el error
    /// </summary>
    public string Usuario { get; set; } = null!;

    /// <summary>
    /// Fecha y hora en que ocurrió el error
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Nombre del programa o procedimiento donde ocurrió el error
    /// </summary>
    public string Programa { get; set; } = null!;

    /// <summary>
    /// Número de línea del código donde ocurrió el error
    /// </summary>
    public int? Linea { get; set; }

    /// <summary>
    /// Clase del error: NEGOCIO, TECNICO, VALIDACION
    /// </summary>
    public string? Clase { get; set; }

    /// <summary>
    /// Código del error: SQLCODE o código de negocio
    /// </summary>
    public decimal? Codigo { get; set; }

    /// <summary>
    /// Mensaje descriptivo del error
    /// </summary>
    public string Mensaje { get; set; } = null!;

    /// <summary>
    /// Pila de llamadas del error formateada
    /// </summary>
    public string? Stack { get; set; }

    /// <summary>
    /// Rastreo de llamadas del error formateada
    /// </summary>
    public string? Backtrace { get; set; }
}
