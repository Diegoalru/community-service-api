using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

public partial class LogError
{
    public int IdLogError { get; set; }

    public string Usuario { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public string Programa { get; set; } = null!;

    public int? Linea { get; set; }

    public string? Clase { get; set; }

    public decimal? Codigo { get; set; }

    public string Mensaje { get; set; } = null!;

    public string? Stack { get; set; }

    public string? Backtrace { get; set; }
}
