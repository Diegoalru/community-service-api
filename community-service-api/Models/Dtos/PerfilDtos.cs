using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class PerfilDto
{
    public int IdPerfil { get; set; }

    public string Identificacion { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string ApellidoP { get; set; } = string.Empty;

    public string? ApellidoM { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public string? Bibliografia { get; set; }

    public char Estado { get; set; }
}

public class PerfilCreateDto
{
    [Required]
    public int IdUsuario { get; set; }

    [Required]
    public int IdUbicacion { get; set; }

    [Required]
    public int IdIdentificador { get; set; }

    [Required]
    [MaxLength(100)]
    public string Identificacion { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ApellidoP { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? ApellidoM { get; set; }

    [Required]
    public DateTime FechaNacimiento { get; set; }

    [MaxLength(2000)]
    public string? Bibliografia { get; set; }

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class PerfilUpdateDto : PerfilCreateDto
{
}
