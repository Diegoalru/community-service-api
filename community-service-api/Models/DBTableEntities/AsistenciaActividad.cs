using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace community_service_api.Models.DBTableEntities
{
    [Table("ASISTENCIA_ACTIVIDAD")]
    public class AsistenciaActividad
    {
        [Key]
        [Column("ID_ASISTENCIA_ACTIVIDAD")]
        public int IdAsistenciaActividad { get; set; }

        [Column("ID_ACTIVIDAD")]
        public int ActividadId { get; set; }

        [Column("ID_USUARIO")]
        public int UsuarioId { get; set; }

        [Column("FECHA")]
        public DateTime Fecha { get; set; }

        [Column("HORAS")]
        public decimal Horas { get; set; }

        [Column("OBSERVACION")]
        public string? Observacion { get; set; }

        [Column("FECHA_CREACION")]
        public DateTime CreadoEn { get; set; }

        [Column("FECHA_MODIFICACION")]
        public DateTime? ActualizadoEn { get; set; }
    }
}
