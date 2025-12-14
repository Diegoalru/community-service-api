using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tipos de identificadores de usuarios
/// </summary>
public partial class TipoIdentificador
{
    public int IdIdentificador { get; set; }

    /// <summary>
    /// Descripción del tipo de identificador (e.g., Cédula Física, Cédula Jurídica, DIMEX, Pasaporte)
    /// </summary>
    public string Descripcion { get; set; } = null!;

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

    public virtual ICollection<Perfil> Perfil { get; set; } = new List<Perfil>();
}
