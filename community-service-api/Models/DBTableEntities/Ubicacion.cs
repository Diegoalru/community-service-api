using System;
using System.Collections.Generic;
using community_service_api.Models.DBTableEntities;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena las ubicaciones de los usuarios
/// </summary>
public partial class Ubicacion
{
    /// <summary>
    /// Identificador único de la ubicación
    /// </summary>
    public int IdUbicacion { get; set; }

    /// <summary>
    /// Identificador del país de la ubicación
    /// </summary>
    public int IdPais { get; set; }

    /// <summary>
    /// Identificador de la provincia de la ubicación
    /// </summary>
    public int? IdProvincia { get; set; }

    /// <summary>
    /// Identificador del cantón de la ubicación
    /// </summary>
    public int? IdCanton { get; set; }

    /// <summary>
    /// Identificador del distrito de la ubicación
    /// </summary>
    public int? IdDistrito { get; set; }

    /// <summary>
    /// Dirección detallada de la ubicación
    /// </summary>
    public string? Direccion { get; set; }

    /// <summary>
    /// Código postal de la ubicación
    /// </summary>
    public string? CodigoPostal { get; set; }

    /// <summary>
    /// Latitud geográfica de la ubicación
    /// </summary>
    public decimal? Latitud { get; set; }

    /// <summary>
    /// Longitud geográfica de la ubicación
    /// </summary>
    public decimal? Longitud { get; set; }

    /// <summary>
    /// Fecha desde la cual el registro es accesible
    /// </summary>
    public DateTime FechaDesde { get; set; }

    /// <summary>
    /// Fecha hasta la cual el registro es accesible
    /// </summary>
    public DateTime? FechaHasta { get; set; }

    /// <summary>
    /// Estado de la ubicación: Activa (A) o Inactiva (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual ICollection<Actividad> Actividad { get; set; } = new List<Actividad>();

    public virtual Distrito? Distrito { get; set; }

    public virtual ICollection<Organizacion> Organizacion { get; set; } = new List<Organizacion>();

    public virtual ICollection<Perfil> Perfil { get; set; } = new List<Perfil>();
}
