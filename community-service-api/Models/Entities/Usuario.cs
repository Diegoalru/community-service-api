using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Entities;

public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    [Required]
    [MaxLength(200)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [MaxLength(200)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public DateTime FechaDesde { get; set; } = DateTime.UtcNow;

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}
