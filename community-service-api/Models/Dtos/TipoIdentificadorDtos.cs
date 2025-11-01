using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class TipoIdentificadorDto
{
    public int IdIdentificador { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public char Estado { get; set; }
}

public class TipoIdentificadorCreateDto
{
    [Required]
    public int IdIdentificador { get; set; }

    [Required]
    [MaxLength(100)]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class TipoIdentificadorUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}
