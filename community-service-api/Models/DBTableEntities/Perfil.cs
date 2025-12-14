using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena los perfiles de los usuarios
/// </summary>
public partial class Perfil
{
    /// <summary>
    /// Identificador único del perfil
    /// </summary>
    public int IdPerfil { get; set; }

    /// <summary>
    /// Identificador del usuario asociado al perfil
    /// </summary>
    public int IdUsuario { get; set; }

    /// <summary>
    /// Identificador de la ubicación asociada al perfil
    /// </summary>
    public int IdUbicacion { get; set; }

    /// <summary>
    /// Identificador del tipo de identificador asociado al perfil
    /// </summary>
    public int IdIdentificador { get; set; }

    /// <summary>
    /// Identificador de la universidad asociada al perfil
    /// </summary>
    public int? IdUniversidad { get; set; }

    /// <summary>
    /// Valor del identificador del usuario
    /// </summary>
    public string Identificacion { get; set; } = null!;

    /// <summary>
    /// Nombre de la persona
    /// </summary>
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// Primer apellido de la persona
    /// </summary>
    public string ApellidoP { get; set; } = null!;

    /// <summary>
    /// Segundo apellido de la persona
    /// </summary>
    public string? ApellidoM { get; set; }

    /// <summary>
    /// Fecha de nacimiento de la persona
    /// </summary>
    public DateTime FechaNacimiento { get; set; }

    /// <summary>
    /// Carrera de estudio de la persona
    /// </summary>
    public string? Carrera { get; set; }

    /// <summary>
    /// Biografía o descripción del usuario
    /// </summary>
    public string? Bibliografia { get; set; }

    /// <summary>
    /// Fecha desde la cual el registro es accesible
    /// </summary>
    public DateTime FechaDesde { get; set; }

    /// <summary>
    /// Fecha hasta la cual el registro es accesible
    /// </summary>
    public DateTime? FechaHasta { get; set; }

    /// <summary>
    /// Estado del perfil: Activo (A) o Inactivo (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public virtual TipoIdentificador IdIdentificadorNavigation { get; set; } = null!;

    public virtual Ubicacion IdUbicacionNavigation { get; set; } = null!;

    public virtual Universidad? IdUniversidadNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
