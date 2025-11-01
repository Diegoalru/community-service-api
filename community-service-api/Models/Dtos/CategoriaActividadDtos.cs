using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class CategoriaActividadDto
{
    public int IdCategoriaActividad { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public char Estado { get; set; }
}

public class CategoriaActividadCreateDto
{
    [Required]
    public int IdCategoriaActividad { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class CategoriaActividadUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}
