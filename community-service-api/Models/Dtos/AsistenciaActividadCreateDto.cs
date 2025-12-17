using System;

namespace community_service_api.Models.Dtos
{
    public class AsistenciaActividadCreateDto
    {
        public int ActividadId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Horas { get; set; }
        public string? Observacion { get; set; }
    }
}
