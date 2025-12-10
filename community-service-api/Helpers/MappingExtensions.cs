using System;
using community_service_api.Models.DBTableEntities;
using community_service_api.Models.Dtos;

namespace community_service_api.Helpers;

public static class MappingExtensions
{
    // Usuario mappings
    public static UsuarioDto ToDto(this Usuario entity) => new()
    {
        IdUsuario = entity.IdUsuario,
        Username = entity.Username,
        Restablecer = entity.Restablecer[0],
        IntentosFallidos = entity.IntentosFallidos,
        FechaDesbloqueo = entity.FechaDesbloqueo,
        TokenEstado = entity.TokenEstado[0],
        TokenExpiracion = entity.TokenExpiracion,
        Estado = entity.Estado[0]
    };

    public static Usuario ToEntity(this UsuarioCreateDto dto) => new()
    {
        Username = dto.Username,
        Password = dto.Password,
        Restablecer = char.ToString(dto.Restablecer),
        IntentosFallidos = dto.IntentosFallidos,
        FechaDesbloqueo = dto.FechaDesbloqueo,
        Token = dto.Token,
        TokenEstado = char.ToString(dto.TokenEstado),
        TokenExpiracion = dto.TokenExpiracion,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = char.ToString(dto.Estado)
    };

    public static void UpdateFromDto(this Usuario entity, UsuarioUpdateDto dto)
    {
        entity.Username = dto.Username;
        entity.Password = dto.Password;
        entity.Restablecer = char.ToString(dto.Restablecer);
        entity.IntentosFallidos = dto.IntentosFallidos;
        entity.FechaDesbloqueo = dto.FechaDesbloqueo;
        entity.Token = dto.Token;
        entity.TokenEstado = char.ToString(dto.TokenEstado);
        entity.TokenExpiracion = dto.TokenExpiracion;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = char.ToString(dto.Estado);
    }

    // TipoIdentificador mappings
    public static TipoIdentificadorDto ToDto(this TipoIdentificador entity) => new()
    {
        IdIdentificador = entity.IdIdentificador,
        Descripcion = entity.Descripcion,
        Estado = entity.Estado[0],
    };

    public static TipoIdentificador ToEntity(this TipoIdentificadorCreateDto dto) => new()
    {
        Descripcion = dto.Descripcion,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = char.ToString(dto.Estado),
    };

    public static void UpdateFromDto(this TipoIdentificador entity, TipoIdentificadorUpdateDto dto)
    {
        entity.Descripcion = dto.Descripcion;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = char.ToString(dto.Estado);
    }

    // Pais mappings
    public static PaisDto ToDto(this Pais entity) => new()
    {
        IdPais = entity.IdPais,
        Nombre = entity.Nombre,
        Estado = entity.Estado[0]
    };

    public static Pais ToEntity(this PaisCreateDto dto) => new()
    {
        Nombre = dto.Nombre,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = char.ToString(dto.Estado)
    };

    public static void UpdateFromDto(this Pais entity, PaisUpdateDto dto)
    {
        entity.Nombre = dto.Nombre;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = char.ToString(dto.Estado);
    }

    // Universidad mappings
    public static UniversidadDto ToDto(this Universidad entity) => new()
    {
        IdUniversidad = entity.IdUniversidad,
        Nombre = entity.Nombre,
        Siglas = entity.Siglas,
        Estado = entity.Estado[0]
    };

    public static Universidad ToEntity(this UniversidadCreateDto dto) => new()
    {
        Nombre = dto.Nombre,
        Siglas = dto.Siglas,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = char.ToString(dto.Estado)
    };

    public static void UpdateFromDto(this Universidad entity, UniversidadUpdateDto dto)
    {
        entity.Nombre = dto.Nombre;
        entity.Siglas = dto.Siglas;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = char.ToString(dto.Estado);
    }

    // Perfil mappings
    public static PerfilDto ToDto(this Perfil entity) => new()
    {
        IdPerfil = entity.IdPerfil,
        Identificacion = entity.Identificacion,
        Nombre = entity.Nombre,
        ApellidoP = entity.ApellidoP,
        ApellidoM = entity.ApellidoM,
        FechaNacimiento = entity.FechaNacimiento,
        Carrera = entity.Carrera,
        Bibliografia = entity.Bibliografia,
        Estado = entity.Estado[0]
    };

    public static Perfil ToEntity(this PerfilCreateDto dto) => new()
    {
        IdUsuario = dto.IdUsuario,
        IdUbicacion = dto.IdUbicacion,
        IdIdentificador = dto.IdIdentificador,
        IdUniversidad = dto.IdUniversidad,
        Identificacion = dto.Identificacion,
        Nombre = dto.Nombre,
        ApellidoP = dto.ApellidoP,
        ApellidoM = dto.ApellidoM,
        FechaNacimiento = dto.FechaNacimiento,
        Carrera = dto.Carrera,
        Bibliografia = dto.Bibliografia,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = char.ToString(dto.Estado)
    };

    public static void UpdateFromDto(this Perfil entity, PerfilUpdateDto dto)
    {
        entity.IdUsuario = dto.IdUsuario;
        entity.IdUbicacion = dto.IdUbicacion;
        entity.IdIdentificador = dto.IdIdentificador;
        entity.IdUniversidad = dto.IdUniversidad;
        entity.Identificacion = dto.Identificacion;
        entity.Nombre = dto.Nombre;
        entity.ApellidoP = dto.ApellidoP;
        entity.ApellidoM = dto.ApellidoM;
        entity.FechaNacimiento = dto.FechaNacimiento;
        entity.Carrera = dto.Carrera;
        entity.Bibliografia = dto.Bibliografia;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = char.ToString(dto.Estado);
    }

    // Organizacion mappings
    public static OrganizacionDto ToDto(this Organizacion entity) => new()
    {
        IdOrganizacion = entity.IdOrganizacion,
        Nombre = entity.Nombre,
        Descripcion = entity.Descripcion,
        Estado = entity.Estado[0]
    };

    public static Organizacion ToEntity(this OrganizacionCreateDto dto) => new()
    {
        IdUsuarioCreador = dto.IdUsuarioCreador,
        IdUbicacion = dto.IdUbicacion,
        IdUniversidad = dto.IdUniversidad,
        Nombre = dto.Nombre,
        Descripcion = dto.Descripcion,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = char.ToString(dto.Estado)
    };

    public static void UpdateFromDto(this Organizacion entity, OrganizacionUpdateDto dto)
    {
        entity.IdUsuarioCreador = dto.IdUsuarioCreador;
        entity.IdUbicacion = dto.IdUbicacion;
        entity.IdUniversidad = dto.IdUniversidad;
        entity.Nombre = dto.Nombre;
        entity.Descripcion = dto.Descripcion;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = char.ToString(dto.Estado);
    }

    // Rol mappings
    public static RolDto ToDto(this Rol entity) => new()
    {
        IdRol = entity.IdRol,
        Nombre = entity.Nombre,
        Estado = entity.Estado[0]
    };

    public static Rol ToEntity(this RolCreateDto dto) => new()
    {
        Nombre = dto.Nombre,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = char.ToString(dto.Estado)
    };

    public static void UpdateFromDto(this Rol entity, RolUpdateDto dto)
    {
        entity.Nombre = dto.Nombre;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = char.ToString(dto.Estado);
    }

    // RolUsuarioOrganizacion mappings
    public static RolUsuarioOrganizacionDto ToDto(this RolUsuarioOrganizacion entity) => new()
    {
        IdRolUsuarioOrganizacion = entity.IdRolUsuarioOrganizacion,
        EsActivo = entity.EsActivo[0],
        Estado = entity.Estado[0]
    };

    public static RolUsuarioOrganizacion ToEntity(this RolUsuarioOrganizacionCreateDto dto) => new()
    {
        IdOrganizacion = dto.IdOrganizacion,
        IdUsuarioAsignado = dto.IdUsuarioAsignado,
        IdUsuarioAdministrador = dto.IdUsuarioAdministrador,
        IdRol = dto.IdRol,
        EsActivo = char.ToString(dto.EsActivo),
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = char.ToString(dto.Estado)
    };

    public static void UpdateFromDto(this RolUsuarioOrganizacion entity, RolUsuarioOrganizacionUpdateDto dto)
    {
        entity.IdOrganizacion = dto.IdOrganizacion;
        entity.IdUsuarioAsignado = dto.IdUsuarioAsignado;
        entity.IdUsuarioAdministrador = dto.IdUsuarioAdministrador;
        entity.IdRol = dto.IdRol;
        entity.EsActivo = char.ToString(dto.EsActivo);
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = char.ToString(dto.Estado);
    }

    // CategoriaActividad mappings
    public static CategoriaActividadDto ToDto(this CategoriaActividad entity) => new()
    {
        IdCategoriaActividad = entity.IdCategoriaActividad,
        Nombre = entity.Nombre,
        Estado = entity.Estado[0]
    };

    public static CategoriaActividad ToEntity(this CategoriaActividadCreateDto dto) => new()
    {
        Nombre = dto.Nombre,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = char.ToString(dto.Estado)
    };

    public static void UpdateFromDto(this CategoriaActividad entity, CategoriaActividadUpdateDto dto)
    {
        entity.Nombre = dto.Nombre;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = char.ToString(dto.Estado);
    }

    // Actividad mappings
    public static ActividadDto ToDto(this Actividad entity) => new()
    {
        IdActividad = entity.IdActividad,
        Nombre = entity.Nombre,
        Descripcion = entity.Descripcion,
        FechaInicio = entity.FechaInicio,
        FechaFin = entity.FechaFin,
        Horas = entity.Horas,
        Cupos = entity.Cupos,
        Situacion = entity.Situacion[0],
        Estado = entity.Estado[0]
    };

    public static Actividad ToEntity(this ActividadCreateDto dto) => new()
    {
        IdOrganizacion = dto.IdOrganizacion,
        IdUsuarioCreador = dto.IdUsuarioCreador,
        IdCategoria = dto.IdCategoria,
        IdUbicacion = dto.IdUbicacion,
        Nombre = dto.Nombre,
        Descripcion = dto.Descripcion,
        FechaInicio = dto.FechaInicio,
        FechaFin = dto.FechaFin,
        Horas = dto.Horas,
        Cupos = dto.Cupos,
        Situacion = char.ToString(dto.Situacion),
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = char.ToString(dto.Estado)
    };

    public static void UpdateFromDto(this Actividad entity, ActividadUpdateDto dto)
    {
        entity.IdOrganizacion = dto.IdOrganizacion;
        entity.IdUsuarioCreador = dto.IdUsuarioCreador;
        entity.IdCategoria = dto.IdCategoria;
        entity.IdUbicacion = dto.IdUbicacion;
        entity.Nombre = dto.Nombre;
        entity.Descripcion = dto.Descripcion;
        entity.FechaInicio = dto.FechaInicio;
        entity.FechaFin = dto.FechaFin;
        entity.Horas = dto.Horas;
        entity.Cupos = dto.Cupos;
        entity.Situacion = char.ToString(dto.Situacion);
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = char.ToString(dto.Estado);
    }

    // CoordinadorActividad mappings
    public static CoordinadorActividadDto ToDto(this CoordinadorActividad entity) => new()
    {
        IdCoordinadorActividad = entity.IdCoordinadorActividad,
        Estado = entity.Estado[0]
    };

    public static CoordinadorActividad ToEntity(this CoordinadorActividadCreateDto dto) => new()
    {
        IdOrganizacion = dto.IdOrganizacion,
        IdActividad = dto.IdActividad,
        IdUsuarioCoordinador = dto.IdUsuarioCoordinador,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = char.ToString(dto.Estado)
    };

    public static void UpdateFromDto(this CoordinadorActividad entity, CoordinadorActividadUpdateDto dto)
    {
        entity.IdOrganizacion = dto.IdOrganizacion;
        entity.IdActividad = dto.IdActividad;
        entity.IdUsuarioCoordinador = dto.IdUsuarioCoordinador;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = char.ToString(dto.Estado);
    }

    // HorarioActividad mappings
    public static HorarioActividadDto ToDto(this HorarioActividad entity) => new()
    {
        IdHorarioActividad = entity.IdHorarioActividad,
        Fecha = entity.Fecha,
        HoraInicio = entity.HoraInicio,
        HoraFin = entity.HoraFin,
        Descripcion = entity.Descripcion,
        Situacion = entity.Situacion[0],
        Estado = entity.Estado[0]
    };

    public static HorarioActividad ToEntity(this HorarioActividadCreateDto dto) => new()
    {
        IdOrganizacion = dto.IdOrganizacion,
        IdActividad = dto.IdActividad,
        IdUsuario = dto.IdUsuario,
        Fecha = dto.Fecha,
        HoraInicio = dto.HoraInicio,
        HoraFin = dto.HoraFin,
        Descripcion = dto.Descripcion,
        Situacion = char.ToString(dto.Situacion),
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = char.ToString(dto.Estado)
    };

    public static void UpdateFromDto(this HorarioActividad entity, HorarioActividadUpdateDto dto)
    {
        entity.IdOrganizacion = dto.IdOrganizacion;
        entity.IdActividad = dto.IdActividad;
        entity.IdUsuario = dto.IdUsuario;
        entity.Fecha = dto.Fecha;
        entity.HoraInicio = dto.HoraInicio;
        entity.HoraFin = dto.HoraFin;
        entity.Descripcion = dto.Descripcion;
        entity.Situacion = char.ToString(dto.Situacion);
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = char.ToString(dto.Estado);
    }

    // ParticipanteActividad mappings
    public static ParticipanteActividadDto ToDto(this ParticipanteActividad entity) => new()
    {
        IdParticipanteActividad = entity.IdParticipanteActividad,
        FechaInscripcion = entity.FechaInscripcion,
        FechaRetiro = entity.FechaRetiro,
        Situacion = entity.Situacion[0],
        Estado = entity.Estado[0]
    };

    public static ParticipanteActividad ToEntity(this ParticipanteActividadCreateDto dto) => new()
    {
        IdOrganizacion = dto.IdOrganizacion,
        IdActividad = dto.IdActividad,
        IdHorarioActividad = dto.IdHorarioActividad,
        IdUsuarioVoluntario = dto.IdUsuarioVoluntario,
        FechaInscripcion = dto.FechaInscripcion,
        FechaRetiro = dto.FechaRetiro,
        Situacion = char.ToString(dto.Situacion),
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = char.ToString(dto.Estado)
    };

    public static void UpdateFromDto(this ParticipanteActividad entity, ParticipanteActividadUpdateDto dto)
    {
        entity.IdOrganizacion = dto.IdOrganizacion;
        entity.IdActividad = dto.IdActividad;
        entity.IdHorarioActividad = dto.IdHorarioActividad;
        entity.IdUsuarioVoluntario = dto.IdUsuarioVoluntario;
        entity.FechaInscripcion = dto.FechaInscripcion;
        entity.FechaRetiro = dto.FechaRetiro;
        entity.Situacion = char.ToString(dto.Situacion);
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = char.ToString(dto.Estado);
    }

    // CertificadoParticipacion mappings
    public static CertificacionParticipacionDto ToDto(this CertificadoParticipacion entity) => new()
    {
        IdCertificacion = entity.IdCertificacion,
        FechaEmision = entity.FechaEmision,
        HorasTotales = entity.HorasTotales,
        DiasTotales = entity.DiasTotales,
        FechaPrimeraAsistencia = entity.FechaPrimeraAsistencia,
        FechaUltimaAsistencia = entity.FechaUltimaAsistencia,
        Situacion = entity.Situacion,
        Observaciones = entity.Observaciones,
        FechaEnvio = entity.FechaEnvio,
        IntentosEnvio = entity.IntentosEnvio,
        UltimoIntentoEnvio = entity.UltimoIntentoEnvio,
        UltimoErrorEnvio = entity.UltimoErrorEnvio,
        Estado = entity.Estado
    };

    public static CertificadoParticipacion ToEntity(this CertificacionParticipacionCreateDto dto) => new()
    {
        IdParticipanteActividad = dto.IdParticipanteActividad,
        IdActividad = dto.IdActividad,
        IdOrganizacion = dto.IdOrganizacion,
        IdUsuarioVoluntario = dto.IdUsuarioVoluntario,
        FechaEmision = dto.FechaEmision,
        HorasTotales = dto.HorasTotales,
        DiasTotales = dto.DiasTotales,
        FechaPrimeraAsistencia = dto.FechaPrimeraAsistencia,
        FechaUltimaAsistencia = dto.FechaUltimaAsistencia,
        Situacion = dto.Situacion,
        Observaciones = dto.Observaciones,
        FechaEnvio = dto.FechaEnvio,
        IntentosEnvio = dto.IntentosEnvio,
        UltimoIntentoEnvio = dto.UltimoIntentoEnvio,
        UltimoErrorEnvio = dto.UltimoErrorEnvio,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = dto.Estado,
    };

    public static void UpdateFromDto(this CertificadoParticipacion entity, CertificacionParticipacionUpdateDto dto)
    {
        entity.IdParticipanteActividad = dto.IdParticipanteActividad;
        entity.IdActividad = dto.IdActividad;
        entity.IdOrganizacion = dto.IdOrganizacion;
        entity.IdUsuarioVoluntario = dto.IdUsuarioVoluntario;
        entity.FechaEmision = dto.FechaEmision;
        entity.HorasTotales = dto.HorasTotales;
        entity.DiasTotales = dto.DiasTotales;
        entity.FechaPrimeraAsistencia = dto.FechaPrimeraAsistencia;
        entity.FechaUltimaAsistencia = dto.FechaUltimaAsistencia;
        entity.Situacion = dto.Situacion;
        entity.Observaciones = dto.Observaciones;
        entity.FechaEnvio = dto.FechaEnvio;
        entity.IntentosEnvio = dto.IntentosEnvio;
        entity.UltimoIntentoEnvio = dto.UltimoIntentoEnvio;
        entity.UltimoErrorEnvio = dto.UltimoErrorEnvio;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = dto.Estado;
    }
}
