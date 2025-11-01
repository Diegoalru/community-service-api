using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class PaisDto
{
    public int IdPais { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public char Estado { get; set; }
}

public class PaisCreateDto
{
    [Required]
    public int IdPais { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class PaisUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}
