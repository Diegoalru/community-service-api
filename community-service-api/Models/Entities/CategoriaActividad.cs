using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Entities;

public class CategoriaActividad
{
    [Key]
    public int IdCategoriaActividad { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}
