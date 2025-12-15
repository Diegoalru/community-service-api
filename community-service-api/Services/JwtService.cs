using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using community_service_api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace community_service_api.Services;

/// <summary>
/// Interfaz para el servicio de generación y validación de tokens JWT.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Genera un token JWT para el usuario especificado.
    /// </summary>
    /// <param name="idUsuario">ID del usuario autenticado.</param>
    /// <param name="username">Nombre de usuario.</param>
    /// <returns>Token JWT generado.</returns>
    string GenerateToken(int idUsuario, string username);

    /// <summary>
    /// Valida un token JWT y retorna los claims si es válido.
    /// </summary>
    /// <param name="token">Token JWT a validar.</param>
    /// <returns>ClaimsPrincipal si el token es válido, null si no lo es.</returns>
    ClaimsPrincipal? ValidateToken(string token);

    /// <summary>
    /// Extrae el ID del usuario de un token JWT.
    /// </summary>
    /// <param name="token">Token JWT.</param>
    /// <returns>ID del usuario o null si el token es inválido.</returns>
    int? GetUserIdFromToken(string token);
}

/// <summary>
/// Implementación del servicio JWT para autenticación.
/// </summary>
public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<JwtService> _logger;
    private readonly SymmetricSecurityKey _securityKey;

    public JwtService(IOptions<JwtSettings> jwtSettings, ILogger<JwtService> logger)
    {
        _jwtSettings = jwtSettings.Value;
        _logger = logger;

        if (string.IsNullOrEmpty(_jwtSettings.SecretKey) || _jwtSettings.SecretKey.Length < 32)
        {
            throw new InvalidOperationException(
                "La clave secreta JWT debe tener al menos 32 caracteres. " +
                "Configure 'JwtSettings:SecretKey' en appsettings.json");
        }

        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
    }

    /// <inheritdoc />
    public string GenerateToken(int idUsuario, string username)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, idUsuario.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("id_usuario", idUsuario.ToString()),
            new("username", username)
        };

        var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        _logger.LogInformation(
            "Token JWT generado para usuario {Username} (ID: {IdUsuario}). Expira en {Minutes} minutos.",
            username, idUsuario, _jwtSettings.ExpirationMinutes);

        return tokenHandler.WriteToken(token);
    }

    /// <inheritdoc />
    public ClaimsPrincipal? ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _securityKey,
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogWarning("Token JWT con algoritmo inválido.");
                return null;
            }

            return principal;
        }
        catch (SecurityTokenExpiredException)
        {
            _logger.LogWarning("Token JWT expirado.");
            return null;
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogWarning("Token JWT inválido: {Message}", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado validando token JWT.");
            return null;
        }
    }

    /// <inheritdoc />
    public int? GetUserIdFromToken(string token)
    {
        var principal = ValidateToken(token);
        if (principal == null)
            return null;

        var userIdClaim = principal.FindFirst("id_usuario") ?? principal.FindFirst(JwtRegisteredClaimNames.Sub);

        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            return userId;

        return null;
    }
}

