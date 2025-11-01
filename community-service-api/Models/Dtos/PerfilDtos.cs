using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class PerfilDto
{
    public Guid IdPerfil { get; set; }
    public Guid IdUsuario { get; set; }
    public int IdIdentificador { get; set; }
    public int IdPais { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ApellidoP { get; set; } = string.Empty;
    public string ApellidoM { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string Telefono { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string CodigoPostal { get; set; } = string.Empty;
    public string Bibliografia { get; set; } = string.Empty;
    public DateTime FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public char Estado { get; set; }
}

public class PerfilCreateDto
{
    [Required]
    public Guid IdUsuario { get; set; }

    [Required]
    public int IdIdentificador { get; set; }

    [Required]
    public int IdPais { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ApellidoP { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ApellidoM { get; set; } = string.Empty;

    [Required]
    public DateTime FechaNacimiento { get; set; }

    [Required]
    [MaxLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Ciudad { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Direccion { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string CodigoPostal { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Bibliografia { get; set; } = string.Empty;

    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class PerfilUpdateDto : PerfilCreateDto
{
}
