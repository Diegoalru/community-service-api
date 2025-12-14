using System;
using System.Collections.Generic;
using community_service_api.Models.DBTableEntities;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla que almacena los usuarios del sistema
/// </summary>
public partial class Usuario
{
    public int IdUsuario { get; set; }

    /// <summary>
    /// Nombre de usuario único para autenticación
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Almacena el hash de la contraseña del usuario
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// Indica si el usuario debe restablecer su contraseña en el próximo inicio de sesión (S/N)
    /// </summary>
    public string Restablecer { get; set; } = null!;

    /// <summary>
    /// Fecha desde la cual el registro es accesible
    /// </summary>
    public DateTime FechaDesde { get; set; }

    /// <summary>
    /// Fecha hasta la cual el registro es accesible
    /// </summary>
    public DateTime? FechaHasta { get; set; }

    /// <summary>
    /// Estado del usuario: Activo (A) o Inactivo (I)
    /// </summary>
    public string Estado { get; set; } = null!;

    public int IntentosFallidos { get; set; }

    /// <summary>
    /// Fecha en la que el usuario fue desbloqueado
    /// </summary>
    public DateTime? FechaDesbloqueo { get; set; }

    /// <summary>
    /// Token de autenticación para el usuario
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Estado del token de autenticación (P: Pendiente, V: Válido, E: Expirado, I: Inválido)
    /// </summary>
    public string TokenEstado { get; set; } = null!;

    /// <summary>
    /// Fecha de expiración del token de autenticación
    /// </summary>
    public DateTime? TokenExpiracion { get; set; }

    public virtual ICollection<Actividad> Actividad { get; set; } = new List<Actividad>();

    public virtual ICollection<CertificadoParticipacion> CertificadoParticipacion { get; set; } = new List<CertificadoParticipacion>();

    public virtual ICollection<CoordinadorActividad> CoordinadorActividad { get; set; } = new List<CoordinadorActividad>();

    public virtual ICollection<CorreoPendiente> CorreoPendiente { get; set; } = new List<CorreoPendiente>();

    public virtual ICollection<Correspondencia> Correspondencia { get; set; } = new List<Correspondencia>();

    public virtual ICollection<HorarioActividad> HorarioActividad { get; set; } = new List<HorarioActividad>();

    public virtual ICollection<Organizacion> Organizacion { get; set; } = new List<Organizacion>();

    public virtual ICollection<ParticipanteActividad> ParticipanteActividad { get; set; } = new List<ParticipanteActividad>();

    public virtual ICollection<Perfil> Perfil { get; set; } = new List<Perfil>();

    public virtual ICollection<RolUsuarioOrganizacion> RolUsuarioOrganizacionIdUsuarioAdministradorNavigation { get; set; } = new List<RolUsuarioOrganizacion>();

    public virtual ICollection<RolUsuarioOrganizacion> RolUsuarioOrganizacionIdUsuarioAsignadoNavigation { get; set; } = new List<RolUsuarioOrganizacion>();
}
