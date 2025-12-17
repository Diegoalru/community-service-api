using System;
using System.Collections.Generic;
using community_service_api.Models.DBTableEntities;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena los certificados de participación de los voluntarios
/// </summary>
public partial class CertificadoParticipacion
{
    /// <summary>
    /// Identificador único del certificado de participación
    /// </summary>
    public Guid IdCertificacion { get; set; }

    /// <summary>
    /// Identificador del participante en la actividad
    /// </summary>
    public int IdParticipanteActividad { get; set; }

    /// <summary>
    /// Identificador de la actividad
    /// </summary>
    public int IdActividad { get; set; }

    /// <summary>
    /// Identificador de la organización que emite el certificado
    /// </summary>
    public int IdOrganizacion { get; set; }

    /// <summary>
    /// Identificador del usuario voluntario al que se le emite el certificado
    /// </summary>
    public int IdUsuarioVoluntario { get; set; }

    /// <summary>
    /// Fecha de emisión del certificado
    /// </summary>
    public DateTime FechaEmision { get; set; }

    /// <summary>
    /// Número total de horas certificadas
    /// </summary>
    public int HorasTotales { get; set; }

    /// <summary>
    /// Número total de días certificados
    /// </summary>
    public int DiasTotales { get; set; }

    /// <summary>
    /// Fecha de la primera asistencia del voluntario
    /// </summary>
    public DateTime FechaPrimeraAsistencia { get; set; }

    /// <summary>
    /// Fecha de la última asistencia del voluntario
    /// </summary>
    public DateTime FechaUltimaAsistencia { get; set; }

    /// <summary>
    /// Situación del certificado: Pendiente (P), Generado (G), Emitido (E), Completado (C), Anulado (A)
    /// </summary>
    public string Situacion { get; set; } = null!;

    /// <summary>
    /// Observaciones adicionales sobre el certificado
    /// </summary>
    public string? Observaciones { get; set; }

    /// <summary>
    /// Documento del certificado en formato BLOB
    /// </summary>
    public byte[]? Documento { get; set; }

    /// <summary>
    /// Fecha desde la cual el registro es accesible
    /// </summary>
    public DateTime FechaDesde { get; set; }

    /// <summary>
    /// Fecha hasta la cual el registro es accesible
    /// </summary>
    public DateTime? FechaHasta { get; set; }

    /// <summary>
    /// Estado del certificado: Activo (A) o Inactivo (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual ICollection<CorreoPendiente> CorreoPendiente { get; set; } = new List<CorreoPendiente>();

    public virtual Actividad IdActividadNavigation { get; set; } = null!;

    public virtual ParticipanteActividad IdParticipanteActividadNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioVoluntarioNavigation { get; set; } = null!;
}
