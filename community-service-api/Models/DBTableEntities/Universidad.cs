using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena las universidades
/// </summary>
public partial class Universidad
{
    /// <summary>
    /// Identificador único de la universidad
    /// </summary>
    public int IdUniversidad { get; set; }

    /// <summary>
    /// Nombre de la universidad
    /// </summary>
    public string Nombre { get; set; } = null!;

    public string Siglas { get; set; } = null!;

    /// <summary>
    /// Fecha desde la cual el registro es accesible
    /// </summary>
    public DateTime FechaDesde { get; set; }

    /// <summary>
    /// Fecha hasta la cual el registro es accesible
    /// </summary>
    public DateTime? FechaHasta { get; set; }

    /// <summary>
    /// Estado de la universidad: Activa (A) o Inactiva (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual ICollection<Organizacion> Organizacion { get; set; } = new List<Organizacion>();

    public virtual ICollection<Perfil> Perfil { get; set; } = new List<Perfil>();
}
