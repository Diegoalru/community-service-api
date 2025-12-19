using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using community_service_api.Models.Dtos;

namespace community_service_api.Models;

// Usado para P_CREAR_ORGANIZACION_JSON
public class OrganizacionCreacionDto
{
    [Required]
    [JsonPropertyName("idUsuarioCreador")]
    public int IdUsuarioCreador { get; set; }

    [Required(ErrorMessage = "Los datos de ubicación son requeridos.")]
    [JsonPropertyName("ubicacion")]
    public UbicacionRegDto Ubicacion { get; set; } = new();

    [Required]
    [StringLength(255)]
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(255)]
    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }
    
    [JsonPropertyName("idUniversidad")]
    public int? IdUniversidad { get; set; }

    [JsonPropertyName("correspondencia")]
    public List<CorrespondenciaRegDto> Correspondencia { get; set; } = new();
}

// Usado para P_CREAR_ACTIVIDAD_JSON
public class ActividadCreacionIntegracionDto
{
    [Required]
    [JsonPropertyName("idUsuarioSolicitante")]
    public int IdUsuarioSolicitante { get; set; }
    
    [Required]
    [JsonPropertyName("actividad")]
    public ActividadDetalleParaCreacionDto Actividad { get; set; } = null!;
}

public class ActividadDetalleParaCreacionDto
{
    [Required]
    [JsonPropertyName("idOrganizacion")]
    public int IdOrganizacion { get; set; }
    [Required]
    [JsonPropertyName("idCategoria")]
    public int IdCategoria { get; set; }
    
    [Required]
    [JsonPropertyName("ubicacion")]
    public UbicacionRegDto Ubicacion { get; set; } = new();

    [Required]
    [StringLength(100)]
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;
    [StringLength(1000)]
    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }
    [Required]
    [JsonPropertyName("fechaInicio")]
    public DateTime FechaInicio { get; set; }
    [Required]
    [JsonPropertyName("fechaFin")]
    public DateTime FechaFin { get; set; }
    [Required]
    [JsonPropertyName("horas")]
    public int Horas { get; set; }
    [JsonPropertyName("cupos")]
    public int? Cupos { get; set; }
}

public class CoordinadorAsignacionDto
{
    [Required]
    [JsonPropertyName("idUsuarioCoordinador")]
    public int IdUsuarioCoordinador { get; set; }
}

// Usado para P_CREAR_HORARIO_JSON
public class HorarioCreacionIntegracionDto
{
    [Required]
    [JsonPropertyName("idUsuarioSolicitante")]
    public int IdUsuarioSolicitante { get; set; }
    [Required]
    [JsonPropertyName("idOrganizacion")]
    public int IdOrganizacion { get; set; }
    [Required]
    [JsonPropertyName("idActividad")]
    public int IdActividad { get; set; }
    [Required]
    [JsonPropertyName("horario")]
    public HorarioDetalleDto Horario { get; set; } = null!;
}

public class HorarioDetalleDto
{
    [Required]
    [JsonPropertyName("fecha")]
    public DateTime Fecha { get; set; }
    [Required]
    [JsonPropertyName("horaInicio")]
    public DateTime HoraInicio { get; set; }
    [Required]
    [JsonPropertyName("horaFin")]
    public DateTime HoraFin { get; set; }
    [StringLength(255)]
    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }
}

// Usado para P_INSCRIBIR_PARTICIPANTE_JSON
public class InscripcionParticipanteDto
{
    [Required]
    [JsonPropertyName("idUsuarioVoluntario")]
    public int IdUsuarioVoluntario { get; set; }
    [Required]
    [JsonPropertyName("idOrganizacion")]
    public int IdOrganizacion { get; set; }
    [Required]
    [JsonPropertyName("idActividad")]
    public int IdActividad { get; set; }
    [JsonPropertyName("idHorarioActividad")]
    public int? IdHorarioActividad { get; set; }
}

// Usado para P_ASIGNAR_ROL_JSON
public class AsignacionRolDto
{
    [Required]
    [JsonPropertyName("idUsuarioSolicitante")]
    public int IdUsuarioSolicitante { get; set; }
    [Required]
    [JsonPropertyName("idOrganizacion")]
    public int IdOrganizacion { get; set; }
    [Required]
    [JsonPropertyName("idUsuarioAsignado")]
    public int IdUsuarioAsignado { get; set; }
    [Required]
    [JsonPropertyName("idRol")]
    public int IdRol { get; set; }
    [Required]
    [JsonPropertyName("esActivo")]
    public bool EsActivo { get; set; }
}

// Usado para P_ACTUALIZAR_PERFIL_COMPLETO_JSON
public class ActualizacionPerfilCompletoDto
{
    [Required]
    [JsonPropertyName("idUsuarioSolicitante")]
    public int IdUsuarioSolicitante { get; set; }
    [Required]
    [JsonPropertyName("perfil")]
    public PerfilActDto Perfil { get; set; } = new();
    [Required]
    [JsonPropertyName("ubicacion")]
    public UbicacionActDto Ubicacion { get; set; } = new();
    [JsonPropertyName("correspondencia")]
    public List<CorrespondenciaRegDto> Correspondencia { get; set; } = new();
}

public class PerfilActDto
{
    [Required]
    [JsonPropertyName("idPerfil")]
    public int IdPerfil { get; set; }
    [JsonPropertyName("idUniversidad")]
    public int? IdUniversidad { get; set; }
    [StringLength(100)]
    [JsonPropertyName("carrera")]
    public string? Carrera { get; set; }
    [StringLength(4000)]
    [JsonPropertyName("bibliografia")]
    public string? Bibliografia { get; set; }
}

public class UbicacionActDto
{
    [Required]
    [JsonPropertyName("idUbicacion")]
    public int IdUbicacion { get; set; }
    [Required]
    [JsonPropertyName("idPais")]
    public int IdPais { get; set; }
    [Required]
    [JsonPropertyName("idProvincia")]
    public int IdProvincia { get; set; }
    [Required]
    [JsonPropertyName("idCanton")]
    public int IdCanton { get; set; }
    [Required]
    [JsonPropertyName("idDistrito")]
    public int IdDistrito { get; set; }
    [StringLength(255)]
    [JsonPropertyName("direccion")]
    public string? Direccion { get; set; }
}

// Usado para P_ACTUALIZAR_ACTIVIDAD_JSON
public class ActividadActualizacionIntegracionDto
{
    [Required]
    [JsonPropertyName("idUsuarioSolicitante")]
    public int IdUsuarioSolicitante { get; set; }
    [Required]
    [JsonPropertyName("actividad")]
    public ActividadDetalleParaActualizacionDto Actividad { get; set; } = null!;
    [JsonPropertyName("coordinadores")]
    public List<CoordinadorAsignacionDto> Coordinadores { get; set; } = new();
}

public class ActividadDetalleParaActualizacionDto
{
    [Required]
    [JsonPropertyName("idActividad")]
    public int IdActividad { get; set; }
    [Required]
    [JsonPropertyName("idCategoria")]
    public int IdCategoria { get; set; }
    
    [Required]
    [JsonPropertyName("ubicacion")]
    public UbicacionActDto Ubicacion { get; set; } = new();

    [Required]
    [StringLength(100)]
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;
    [StringLength(1000)]
    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }
    [Required]
    [JsonPropertyName("fechaInicio")]
    public DateTime FechaInicio { get; set; }
    [Required]
    [JsonPropertyName("fechaFin")]
    public DateTime FechaFin { get; set; }
    [Required]
    [JsonPropertyName("horas")]
    public int Horas { get; set; }
    [JsonPropertyName("cupos")]
    public int? Cupos { get; set; }
}
