using System;
using System.Collections.Generic;

namespace community_service_api.Models.Dtos;

public class MisHorasDto
{
    public decimal HorasTotales { get; set; }
    public int Actividades { get; set; }
    public DateTime? UltimaParticipacion { get; set; }
    public List<MisHorasDetalleDto> Desglose { get; set; } = new();
}

public class MisHorasDetalleDto
{
    public int IdOrganizacion { get; set; }
    public int IdActividad { get; set; }
    public string Actividad { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public decimal Horas { get; set; }
}


