using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class ActivationDto
{
    [Required]
    public required string Token { get; set; }
}

public class RequestPasswordRecoveryDto
{
    [Required]
    public required string Username { get; set; }
}

public class ResetPasswordDto
{
    [Required]
    public required string Token { get; set; }

    [Required]
    [MinLength(6)]
    public required string NuevaPassword { get; set; }
}

public class ChangePasswordDto
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }

    [Required]
    [MinLength(6)]
    public required string NuevaPassword { get; set; }
}

public class ResendActivationDto
{
    [Required]
    public required string Username { get; set; }
}
