using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class UsuarioDto
{
    public int IdUsuario { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public char Estado { get; set; }
}

public class UsuarioCreateDto
{
    [Required]
    [MaxLength(200)]
    public string Usuario { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [MaxLength(200)]
    public string Password { get; set; } = string.Empty;

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
    public string Usuario { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [MaxLength(200)]
    public string Password { get; set; } = string.Empty;
}

public class UsuarioUpdateDto : UsuarioCreateDto
{
}
