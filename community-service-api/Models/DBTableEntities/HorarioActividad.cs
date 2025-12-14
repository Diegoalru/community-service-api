using System;
using System.Collections.Generic;
using community_service_api.Models.DBTableEntities;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena los horarios de las actividades
/// </summary>
public partial class HorarioActividad
{
    /// <summary>
    /// Identificador único del horario de la actividad
    /// </summary>
    public int IdHorarioActividad { get; set; }

    /// <summary>
    /// Identificador de la organización que crea el horario
    /// </summary>
    public int IdOrganizacion { get; set; }

    /// <summary>
    /// Identificador de la actividad
    /// </summary>
    public int IdActividad { get; set; }

    /// <summary>
    /// Identificador del usuario que crea el horario
    /// </summary>
    public int IdUsuario { get; set; }

    /// <summary>
    /// Fecha del horario de la actividad
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Hora de inicio del horario de la actividad
    /// </summary>
    public DateTime HoraInicio { get; set; }

    /// <summary>
    /// Hora de fin del horario de la actividad
    /// </summary>
    public DateTime HoraFin { get; set; }

    /// <summary>
    /// Descripción del horario de la actividad
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Situación del horario de la actividad: Iniciada (I), Publicada (P), Cancelada (C), Finalizada (F), Anulada (A)
    /// </summary>
    public string Situacion { get; set; } = null!;

    /// <summary>
    /// Fecha desde la cual el registro es accesible
    /// </summary>
    public DateTime FechaDesde { get; set; }

    /// <summary>
    /// Fecha hasta la cual el registro es accesible
    /// </summary>
    public DateTime? FechaHasta { get; set; }

    /// <summary>
    /// Estado del registro: Activo (A) o Inactivo (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual Actividad Actividad { get; set; } = null!;

    public virtual Organizacion IdOrganizacionNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<ParticipanteActividad> ParticipanteActividad { get; set; } = new List<ParticipanteActividad>();
}
