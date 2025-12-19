using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace community_service_api.Models.Dtos;

// Para P_GET_ORGANIZACIONES_CON_ESTADO_JSON
public class GetOrganizacionesConEstadoDto
{
    [Required]
    [JsonPropertyName("idUsuario")]
    public int IdUsuario { get; set; }
}

// Para P_GESTIONAR_VOLUNTARIADO_JSON
public class GestionarVoluntariadoDto
{
    [Required]
    [JsonPropertyName("idUsuario")]
    public int IdUsuario { get; set; }
    
    [Required]
    [JsonPropertyName("idOrganizacion")]
    public int IdOrganizacion { get; set; }

    [Required]
    [JsonPropertyName("accion")]
    public string Accion { get; set; } = string.Empty; // "suscribir" o "desuscribir"
}

// Para P_GET_USUARIOS_POR_ORG_JSON
public class GetUsuariosPorOrgDto
{
    [Required]
    [JsonPropertyName("idOrganizacion")]
    public int IdOrganizacion { get; set; }
}

// Para P_ELIMINAR_USUARIO_ORG_JSON
public class EliminarUsuarioOrgDto
{
    [Required]
    [JsonPropertyName("idUsuarioSolicitante")]
    public int IdUsuarioSolicitante { get; set; }

    [Required]
    [JsonPropertyName("idRolUsuarioOrganizacion")]
    public int IdRolUsuarioOrganizacion { get; set; }
}

// Para P_ACTUALIZAR_USUARIO_ORG_JSON
public class ActualizarUsuarioOrgDto
{
    [Required]
    [JsonPropertyName("idUsuarioSolicitante")]
    public int IdUsuarioSolicitante { get; set; }

    [Required]
    [JsonPropertyName("idRolUsuarioOrganizacion")]
    public int IdRolUsuarioOrganizacion { get; set; }

    [Required]
    [JsonPropertyName("esActivo")]
    public bool EsActivo { get; set; }
}

// Para P_GET_ACTIVIDADES_POR_ORG_JSON
public class GetActividadesPorOrgDto
{
    [Required]
    [JsonPropertyName("idOrganizacion")]
    public int IdOrganizacion { get; set; }
}

// Para P_GET_HORARIOS_POR_ACT_JSON
public class GetHorariosPorActDto
{
    [Required]
    [JsonPropertyName("idActividad")]
    public int IdActividad { get; set; }
}

// Para P_ELIMINAR_HORARIO_JSON
public class EliminarHorarioDto
{
    [Required]
    [JsonPropertyName("idUsuarioSolicitante")]
    public int IdUsuarioSolicitante { get; set; }

    [Required]
    [JsonPropertyName("idHorarioActividad")]
    public int IdHorarioActividad { get; set; }
}

// Para P_ELIMINAR_ACTIVIDAD_JSON
public class EliminarActividadDto
{
    [Required]
    [JsonPropertyName("idUsuarioSolicitante")]
    public int IdUsuarioSolicitante { get; set; }

    [Required]
    [JsonPropertyName("idActividad")]
    public int IdActividad { get; set; }
}

// Para P_ACTUALIZAR_HORARIO_JSON
public class ActualizarHorarioDto
{
    [Required]
    [JsonPropertyName("idUsuarioSolicitante")]
    public int IdUsuarioSolicitante { get; set; }

    [Required]
    [JsonPropertyName("idHorarioActividad")]
    public int IdHorarioActividad { get; set; }

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

// Para los procedimientos GET por ID
public class GetByIdDto
{
    [Required]
    [JsonPropertyName("id")]
    public int Id { get; set; }
}

// Para P_ACTUALIZAR_ORGANIZACION_JSON

public class UbicacionDto
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
    [Required]
    [StringLength(200)]
    [JsonPropertyName("direccion")]
    public string Direccion { get; set; } = string.Empty;
}

public class OrganizacionNestedDto
{
    [Required]
    [JsonPropertyName("idOrganizacion")]
    public int IdOrganizacion { get; set; }
    [Required]
    [StringLength(255)]
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;
    [Required]
    [StringLength(4000)]
    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; } = string.Empty;
    [Required]
    [JsonPropertyName("idUniversidad")]
    public int IdUniversidad { get; set; }
}
public class ActualizarOrganizacionDto
{
    [Required]
    [JsonPropertyName("idUsuarioSolicitante")]
    public int IdUsuarioSolicitante { get; set; }
    
    [Required]
    [JsonPropertyName("organizacion")]
    public OrganizacionNestedDto Organizacion { get; set; } = null!;

    [Required]
    [JsonPropertyName("ubicacion")]
    public UbicacionDto Ubicacion { get; set; } = null!;
}

// Para P_CAMBIAR_ROL_USUARIO_JSON
public class CambiarRolUsuarioDto
{
    [Required]
    [JsonPropertyName("idUsuarioSolicitante")]
    public int IdUsuarioSolicitante { get; set; }
    [Required]
    [JsonPropertyName("idRolUsuarioOrganizacion")]
    public int IdRolUsuarioOrganizacion { get; set; }
    [Required]
    [JsonPropertyName("idNuevoRol")]
    public int IdNuevoRol { get; set; }
}

// Definition for RolUsuarioApi
public class RolUsuarioApi
{
    [JsonPropertyName("idRol")]
    public int IdRol { get; set; }
    [JsonPropertyName("nombreRol")]
    public string NombreRol { get; set; } = string.Empty;
    [JsonPropertyName("esAdmin")]
    public string EsAdmin { get; set; } = string.Empty; // "true" or "false"
}

public class OrganizacionConEstadoApi
{
    [JsonPropertyName("idOrganizacion")]
    public int IdOrganizacion { get; set; }
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;
    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }
    [JsonPropertyName("rolesUsuario")] // Changed to rolesUsuario
    public List<RolUsuarioApi>? RolesUsuario { get; set; } // Changed to List<RolUsuarioApi>
}