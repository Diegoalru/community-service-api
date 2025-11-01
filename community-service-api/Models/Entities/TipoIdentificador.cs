using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Entities;

public class TipoIdentificador
{
    [Key]
    public int IdIdentificador { get; set; }

    [Required]
    [MaxLength(100)]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}
