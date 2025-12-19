using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace community_service_api.Models.Dtos;

public class RegistroCompletoDto
{
    [Required(ErrorMessage = "Los datos de usuario son requeridos.")]
    [JsonPropertyName("usuario")]
    public UsuarioRegDto Usuario { get; set; } = new();

    [Required(ErrorMessage = "Los datos de perfil son requeridos.")]
    [JsonPropertyName("perfil")]
    public PerfilRegDto Perfil { get; set; } = new();

    [Required(ErrorMessage = "Los datos de ubicación son requeridos.")]
    [JsonPropertyName("ubicacion")]
    public UbicacionRegDto Ubicacion { get; set; } = new();

    [Required(ErrorMessage = "Al menos un dato de correspondencia es requerido.")]
    [MinLength(1, ErrorMessage = "Debe proporcionar al menos un dato de correspondencia.")]
    [JsonPropertyName("correspondencia")]
    public List<CorrespondenciaRegDto> Correspondencia { get; set; } = new();
}

public class UsuarioRegDto
{
    [Required(ErrorMessage = "El nombre de usuario es requerido.")]
    [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder 50 caracteres.")]
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida.")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 50 caracteres.")]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}

public class PerfilRegDto
{
    [Required(ErrorMessage = "El tipo de identificador es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "El tipo de identificador debe ser válido.")]
    [JsonPropertyName("idIdentificador")]
    public int IdIdentificador { get; set; }

    [Required(ErrorMessage = "El número de identificación es requerido.")]
    [StringLength(50, ErrorMessage = "La identificación no puede exceder 50 caracteres.")]
    [JsonPropertyName("identificacion")]
    public string Identificacion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es requerido.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido paterno es requerido.")]
    [StringLength(100, ErrorMessage = "El apellido paterno no puede exceder 100 caracteres.")]
    [JsonPropertyName("apellidoP")]
    public string ApellidoP { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "El apellido materno no puede exceder 100 caracteres.")]
    [JsonPropertyName("apellidoM")]
    public string? ApellidoM { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es requerida.")]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "La fecha de nacimiento debe tener formato YYYY-MM-DD.")]
    [JsonPropertyName("fechaNacimiento")]
    public string FechaNacimiento { get; set; } = string.Empty;

    [JsonPropertyName("idUniversidad")]
    public int? IdUniversidad { get; set; }

    [StringLength(100, ErrorMessage = "La carrera no puede exceder 100 caracteres.")]
    [JsonPropertyName("carrera")]
    public string? Carrera { get; set; }

    [StringLength(4000, ErrorMessage = "La bibliografía no puede exceder 4000 caracteres.")]
    [JsonPropertyName("bibliografia")]
    public string? Bibliografia { get; set; }
}

public class UbicacionRegDto
{
    [Required(ErrorMessage = "El país es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "El país debe ser válido.")]
    [JsonPropertyName("idPais")]
    public int IdPais { get; set; }

    [Required(ErrorMessage = "La provincia es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "La provincia debe ser válida.")]
    [JsonPropertyName("idProvincia")]
    public int IdProvincia { get; set; }

    [Required(ErrorMessage = "El cantón es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "El cantón debe ser válido.")]
    [JsonPropertyName("idCanton")]
    public int IdCanton { get; set; }

    [Required(ErrorMessage = "El distrito es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "El distrito debe ser válido.")]
    [JsonPropertyName("idDistrito")]
    public int IdDistrito { get; set; }

    [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres.")]
    [JsonPropertyName("direccion")]
    public string? Direccion { get; set; }

    [StringLength(20, ErrorMessage = "El código postal no puede exceder 20 caracteres.")]
    [JsonPropertyName("codigoPostal")]
    public string? CodigoPostal { get; set; }

    [JsonPropertyName("latitud")]
    public decimal? Latitud { get; set; }

    [JsonPropertyName("longitud")]
    public decimal? Longitud { get; set; }
}

public class CorrespondenciaRegDto
{
    [Required(ErrorMessage = "El tipo de correspondencia es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "El tipo de correspondencia debe ser válido.")]
    [JsonPropertyName("idTipoCorrespondencia")]
    public int IdTipoCorrespondencia { get; set; }

    [Required(ErrorMessage = "El valor de correspondencia es requerido.")]
    [StringLength(255, ErrorMessage = "El valor no puede exceder 255 caracteres.")]
    [JsonPropertyName("valor")]
    public string Valor { get; set; } = string.Empty;

    [RegularExpression(@"^[SN]$", ErrorMessage = "El consentimiento debe ser 'S' o 'N'.")]
    [JsonPropertyName("consentimiento")]
    public string Consentimiento { get; set; } = "N";
}

public class ReenvioTokenDto
{
    [Required(ErrorMessage = "El nombre de usuario es requerido.")]
    [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder 50 caracteres.")]
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
}