using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class UsuarioDto
{
    public int IdUsuario { get; set; }

    public string Username { get; set; } = string.Empty;

    public char Restablecer { get; set; }

    public int IntentosFallidos { get; set; }

    public DateTime? FechaDesbloqueo { get; set; }

    public char TokenEstado { get; set; }

    public DateTime? TokenExpiracion { get; set; }

    public char Estado { get; set; }
}

public class UsuarioCreateDto
{
    [Required]
    [MaxLength(200)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;

    [RegularExpression("[NS]")]
    public char Restablecer { get; set; } = 'N';

    public int IntentosFallidos { get; set; } = 0;

    public DateTime? FechaDesbloqueo { get; set; }

    [MaxLength(255)]
    public string? Token { get; set; }

    [RegularExpression("[PVEI]")]
    public char TokenEstado { get; set; } = 'V';

    public DateTime? TokenExpiracion { get; set; }

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class UsuarioCreateDtoTest
{
    [Required]
    [MaxLength(200)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;
}

public class UsuarioUpdateDto : UsuarioCreateDto
{
}
