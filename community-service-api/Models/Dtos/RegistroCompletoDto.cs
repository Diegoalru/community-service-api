using System.Text.Json.Serialization;

namespace community_service_api.Models.Dtos;

public class RegistroCompletoDto
{
    [JsonPropertyName("usuario")]
    public UsuarioRegDto Usuario { get; set; } = new();

    [JsonPropertyName("perfil")]
    public PerfilRegDto Perfil { get; set; } = new();

    [JsonPropertyName("ubicacion")]
    public UbicacionRegDto Ubicacion { get; set; } = new();

    [JsonPropertyName("correspondencia")]
    public List<CorrespondenciaRegDto> Correspondencia { get; set; } = new();
}

public class UsuarioRegDto
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}

public class PerfilRegDto
{
    [JsonPropertyName("id_identificador")]
    public int IdIdentificador { get; set; }
    [JsonPropertyName("identificacion")]
    public string Identificacion { get; set; } = string.Empty;
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;
    [JsonPropertyName("apellido_p")]
    public string ApellidoP { get; set; } = string.Empty;
    [JsonPropertyName("apellido_m")]
    public string? ApellidoM { get; set; }
    [JsonPropertyName("fecha_nacimiento")]
    public string FechaNacimiento { get; set; } = string.Empty; // Formato YYYY-MM-DD
    [JsonPropertyName("id_universidad")]
    public int? IdUniversidad { get; set; }
    [JsonPropertyName("carrera")]
    public string? Carrera { get; set; }
    [JsonPropertyName("bibliografia")]
    public string? Bibliografia { get; set; }
}

public class UbicacionRegDto
{
    [JsonPropertyName("id_pais")]
    public int IdPais { get; set; }
    [JsonPropertyName("id_provincia")]
    public int IdProvincia { get; set; }
    [JsonPropertyName("id_canton")]
    public int IdCanton { get; set; }
    [JsonPropertyName("id_distrito")]
    public int IdDistrito { get; set; }
    [JsonPropertyName("direccion")]
    public string? Direccion { get; set; }
    [JsonPropertyName("codigo_postal")]
    public string? CodigoPostal { get; set; }
    [JsonPropertyName("latitud")]
    public decimal? Latitud { get; set; }
    [JsonPropertyName("longitud")]
    public decimal? Longitud { get; set; }
}

public class CorrespondenciaRegDto
{
    [JsonPropertyName("id_tipo_correspondencia")]
    public int IdTipoCorrespondencia { get; set; }
    [JsonPropertyName("valor")]
    public string Valor { get; set; } = string.Empty;
    [JsonPropertyName("consentimiento")]
    public string Consentimiento { get; set; } = "N";
}

public class ReenvioTokenDto
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
}