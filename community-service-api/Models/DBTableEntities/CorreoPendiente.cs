using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

/// <summary>
/// Tabla para encolar correos electrónicos a ser enviados por un proceso en segundo plano.
/// </summary>
public partial class CorreoPendiente
{
    /// <summary>
    /// Identificador único del correo pendiente.
    /// </summary>
    public int IdCorreoPendiente { get; set; }

    /// <summary>
    /// Dirección de correo electrónico del destinatario.
    /// </summary>
    public string Destinatario { get; set; } = null!;

    /// <summary>
    /// Asunto del correo electrónico.
    /// </summary>
    public string Asunto { get; set; } = null!;

    /// <summary>
    /// Cuerpo del correo electrónico, puede contener HTML.
    /// </summary>
    public string Cuerpo { get; set; } = null!;

    /// <summary>
    /// Contenido binario del archivo adjunto (e.g., PDF del certificado).
    /// </summary>
    public byte[]? Adjunto { get; set; }

    /// <summary>
    /// Nombre del archivo adjunto.
    /// </summary>
    public string? NombreAdjunto { get; set; }

    /// <summary>
    /// ID del usuario al que se le envía este correo.
    /// </summary>
    public int IdUsuario { get; set; }

    /// <summary>
    /// ID del certificado que originó este correo, si aplica.
    /// </summary>
    public Guid? IdCertificacion { get; set; }

    /// <summary>
    /// Estado del envío del correo: Pendiente, Enviado, Fallido.
    /// </summary>
    public string Estado { get; set; } = null!;

    /// <summary>
    /// Número de intentos de envío.
    /// </summary>
    public int Intentos { get; set; }

    /// <summary>
    /// Fecha en que se encoló el correo.
    /// </summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// Fecha del último intento de envío.
    /// </summary>
    public DateTime? FechaUltimoIntento { get; set; }

    /// <summary>
    /// Mensaje de error si el último intento de envío falló.
    /// </summary>
    public string? MensajeError { get; set; }

    public virtual CertificadoParticipacion? IdCertificacionNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
