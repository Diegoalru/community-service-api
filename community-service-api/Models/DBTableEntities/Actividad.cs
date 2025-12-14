using community_service_api.Models;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena las actividades registradas en el sistema
/// </summary>
public partial class Actividad
{
    /// <summary>
    /// Identificador único de la actividad
    /// </summary>
    public int IdActividad { get; set; }

    /// <summary>
    /// Identificador de la organización que crea la actividad
    /// </summary>
    public int IdOrganizacion { get; set; }

    /// <summary>
    /// Identificador del usuario que crea la actividad
    /// </summary>
    public int IdUsuarioCreador { get; set; }

    /// <summary>
    /// Identificador de la categoría de la actividad
    /// </summary>
    public int IdCategoria { get; set; }

    /// <summary>
    /// Identificador de la ubicación de la actividad
    /// </summary>
    public int IdUbicacion { get; set; }

    /// <summary>
    /// Nombre de la actividad
    /// </summary>
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// Descripción de la actividad
    /// </summary>
    public string Descripcion { get; set; } = null!;

    /// <summary>
    /// Fecha de inicio de la actividad
    /// </summary>
    public DateTime? FechaInicio { get; set; }

    /// <summary>
    /// Fecha de finalización de la actividad
    /// </summary>
    public DateTime? FechaFin { get; set; }

    /// <summary>
    /// Número de horas de la actividad
    /// </summary>
    public int? Horas { get; set; }

    /// <summary>
    /// Número de cupos disponibles para la actividad
    /// </summary>
    public int Cupos { get; set; }

    /// <summary>
    /// Situación de la actividad: Iniciada (I), Publicada (P), Cancelada (C), Finalizada (F), Anulada (A)
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
    /// Estado de la actividad: Activa (A) o Inactiva (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual ICollection<CertificadoParticipacion> CertificadoParticipacion { get; set; } = new List<CertificadoParticipacion>();

    public virtual ICollection<CoordinadorActividad> CoordinadorActividad { get; set; } = new List<CoordinadorActividad>();

    public virtual ICollection<HorarioActividad> HorarioActividad { get; set; } = new List<HorarioActividad>();

    public virtual CategoriaActividad IdCategoriaNavigation { get; set; } = null!;

    public virtual Organizacion IdOrganizacionNavigation { get; set; } = null!;

    public virtual Ubicacion IdUbicacionNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioCreadorNavigation { get; set; } = null!;

    public virtual ICollection<ParticipanteActividad> ParticipanteActividad { get; set; } = new List<ParticipanteActividad>();
}
