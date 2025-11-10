using System;
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
    [MaxLength(100)]
    public string Descripcion { get; set; } = string.Empty;

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class TipoIdentificadorUpdateDto : TipoIdentificadorCreateDto
{
}
