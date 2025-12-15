using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class ActivationDto
{
    [Required(ErrorMessage = "El token es requerido.")]
    [StringLength(500, ErrorMessage = "El token no puede exceder 500 caracteres.")]
    public required string Token { get; set; }
}

public class RequestPasswordRecoveryDto
{
    [Required(ErrorMessage = "El nombre de usuario es requerido.")]
    [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder 50 caracteres.")]
    public required string Username { get; set; }
}

public class ResetPasswordDto
{
    [Required(ErrorMessage = "El token es requerido.")]
    [StringLength(500, ErrorMessage = "El token no puede exceder 500 caracteres.")]
    public required string Token { get; set; }

    [Required(ErrorMessage = "La nueva contraseña es requerida.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    [MaxLength(50, ErrorMessage = "La contraseña no puede exceder 50 caracteres.")]
    public required string NuevaPassword { get; set; }
}

public class ChangePasswordDto
{
    [Required(ErrorMessage = "El nombre de usuario es requerido.")]
    [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder 50 caracteres.")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "La contraseña actual es requerida.")]
    [StringLength(50, ErrorMessage = "La contraseña no puede exceder 50 caracteres.")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "La nueva contraseña es requerida.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    [MaxLength(50, ErrorMessage = "La contraseña no puede exceder 50 caracteres.")]
    public required string NuevaPassword { get; set; }
}

public class ResendActivationDto
{
    [Required(ErrorMessage = "El nombre de usuario es requerido.")]
    [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder 50 caracteres.")]
    public required string Username { get; set; }
}
