namespace community_service_api.Models;

/// <summary>
/// Configuración para la generación y validación de tokens JWT.
/// Los valores de Issuer y Audience son identificadores lógicos, no URLs ni IPs.
/// Funcionan igual en desarrollo local y producción siempre que coincidan
/// entre el emisor (API) y el consumidor (cliente web).
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Clave secreta para firmar los tokens. Debe tener al menos 32 caracteres.
    /// IMPORTANTE: Usar diferentes claves en Development y Production.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Emisor del token (issuer). Identifica quién genera el token.
    /// Ejemplo: "CommunityServiceAPI"
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Audiencia del token (audience). Identifica para quién está destinado el token.
    /// Ejemplo: "CommunityServiceClient"
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Tiempo de expiración del token en minutos.
    /// Valor por defecto: 120 minutos (2 horas).
    /// </summary>
    public int ExpirationMinutes { get; set; } = 120;
}

