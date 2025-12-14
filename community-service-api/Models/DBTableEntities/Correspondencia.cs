using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla de correspondencia de datos para usuarios y organizaciones
/// </summary>
public partial class Correspondencia
{
    public int IdCorrespondencia { get; set; }

    /// <summary>
    /// Identificador del usuario (si la correspondencia es para un usuario)
    /// </summary>
    public int? IdUsuario { get; set; }

    /// <summary>
    /// Tipo de correspondencia
    /// </summary>
    public int IdTipoCorrespondencia { get; set; }

    /// <summary>
    /// Valor de la correspondencia
    /// </summary>
    public string Valor { get; set; } = null!;

    /// <summary>
    /// Consentimiento del usuario
    /// </summary>
    public string Consentimiento { get; set; } = null!;

    /// <summary>
    /// Fecha desde la cual el registro es accesible
    /// </summary>
    public DateTime FechaDesde { get; set; }

    /// <summary>
    /// Fecha hasta la cual el registro es accesible
    /// </summary>
    public DateTime? FechaHasta { get; set; }

    /// <summary>
    /// Estado de la correspondencia: Activa (A) o Inactiva (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    /// <summary>
    /// Identificador de la organización (si la correspondencia es para una organización)
    /// </summary>
    public int? IdOrganizacion { get; set; }

    public virtual Organizacion? IdOrganizacionNavigation { get; set; }

    public virtual TipoCorrespondencia IdTipoCorrespondenciaNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
