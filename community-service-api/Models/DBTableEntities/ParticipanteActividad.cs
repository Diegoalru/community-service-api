using System;
using System.Collections.Generic;
using community_service_api.Models.DBTableEntities;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que relaciona los participantes con las actividades
/// </summary>
public partial class ParticipanteActividad
{
    /// <summary>
    /// Identificador del registro
    /// </summary>
    public int IdParticipanteActividad { get; set; }

    /// <summary>
    /// Identificador de la organización
    /// </summary>
    public int IdOrganizacion { get; set; }

    /// <summary>
    /// Identificador de la actividad
    /// </summary>
    public int IdActividad { get; set; }

    /// <summary>
    /// Identificador del horario de la actividad
    /// </summary>
    public int IdHorarioActividad { get; set; }

    /// <summary>
    /// Identificador del usuario voluntario participante
    /// </summary>
    public int IdUsuarioVoluntario { get; set; }

    /// <summary>
    /// Fecha de inscripción del participante en la actividad
    /// </summary>
    public DateTime FechaInscripcion { get; set; }

    /// <summary>
    /// Fecha de retiro del participante de la actividad
    /// </summary>
    public DateTime? FechaRetiro { get; set; }

    /// <summary>
    /// Situación del participante en la actividad: Inicial (I), Activo (A), Retirado (R), Cancelado (C), Finalizado (F)
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

    public virtual CertificadoParticipacion? CertificadoParticipacion { get; set; }

    public virtual HorarioActividad HorarioActividad { get; set; } = null!;

    public virtual Organizacion IdOrganizacionNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioVoluntarioNavigation { get; set; } = null!;
}
