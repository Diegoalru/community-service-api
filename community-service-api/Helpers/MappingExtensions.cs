using community_service_api.Models.DBTableEntities;
using community_service_api.Models.Dtos;

namespace community_service_api.Helpers;

public static class MappingExtensions
{
    public static UsuarioDto ToDto(this Usuario entity) => new()
    {
        IdUsuario = entity.IdUsuario,
        Email = entity.Username,
        FechaDesde = entity.FechaDesde,
        FechaHasta = entity.FechaHasta,
        Estado = entity.Estado
    };

    public static Usuario ToEntity(this UsuarioCreateDto dto) => new()
    {
        IdUsuario = 0, // Assuming the database will auto-generate the ID
        Username = dto.Usuario,
        Password = dto.Password,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = dto.Estado
    };

    public static void UpdateFromDto(this Usuario entity, UsuarioUpdateDto dto)
    {
        entity.Username = dto.Usuario;
        entity.Password = dto.Password;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = dto.Estado;
    }

    public static TipoIdentificadorDto ToDto(this TipoIdentificador entity) => new()
    {
        IdIdentificador = entity.IdIdentificador,
        Descripcion = entity.Descripcion,
        Estado = entity.Estado
    };

    public static TipoIdentificador ToEntity(this TipoIdentificadorCreateDto dto) => new()
    {
        IdIdentificador = dto.IdIdentificador,
        Descripcion = dto.Descripcion,
        Estado = dto.Estado
    };

    public static void UpdateFromDto(this TipoIdentificador entity, TipoIdentificadorUpdateDto dto)
    {
        entity.Descripcion = dto.Descripcion;
        entity.Estado = dto.Estado;
    }

    public static PaisDto ToDto(this Pais entity) => new()
    {
        IdPais = entity.IdPais,
        Nombre = entity.Nombre,
        Estado = entity.Estado
    };

    public static Pais ToEntity(this PaisCreateDto dto) => new()
    {
        IdPais = dto.IdPais,
        Nombre = dto.Nombre,
        Estado = dto.Estado
    };

    public static void UpdateFromDto(this Pais entity, PaisUpdateDto dto)
    {
        entity.Nombre = dto.Nombre;
        entity.Estado = dto.Estado;
    }

    public static PerfilDto ToDto(this Perfil entity) => new()
    {
        IdPerfil = entity.IdPerfil,
        IdUsuario = entity.IdUsuario,
        IdIdentificador = entity.IdIdentificador,
        IdPais = entity.IdPais,
        Nombre = entity.Nombre,
        ApellidoP = entity.ApellidoP,
        ApellidoM = entity.ApellidoM,
        FechaNacimiento = entity.FechaNacimiento,
        Telefono = entity.Telefono,
        Ciudad = entity.Ciudad,
        Direccion = entity.Direccion,
        CodigoPostal = entity.CodigoPostal,
        Bibliografia = entity.Bibliografia,
        FechaDesde = entity.FechaDesde,
        FechaHasta = entity.FechaHasta,
        Estado = entity.Estado
    };

    public static Perfil ToEntity(this PerfilCreateDto dto) => new()
    {
        IdPerfil = Guid.NewGuid(),
        IdUsuario = dto.IdUsuario,
        IdIdentificador = dto.IdIdentificador,
        IdPais = dto.IdPais,
        Nombre = dto.Nombre,
        ApellidoP = dto.ApellidoP,
        ApellidoM = dto.ApellidoM,
        FechaNacimiento = dto.FechaNacimiento,
        Telefono = dto.Telefono,
        Ciudad = dto.Ciudad,
        Direccion = dto.Direccion,
        CodigoPostal = dto.CodigoPostal,
        Bibliografia = dto.Bibliografia,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = dto.Estado
    };

    public static void UpdateFromDto(this Perfil entity, PerfilUpdateDto dto)
    {
        entity.IdUsuario = dto.IdUsuario;
        entity.IdIdentificador = dto.IdIdentificador;
        entity.IdPais = dto.IdPais;
        entity.Nombre = dto.Nombre;
        entity.ApellidoP = dto.ApellidoP;
        entity.ApellidoM = dto.ApellidoM;
        entity.FechaNacimiento = dto.FechaNacimiento;
        entity.Telefono = dto.Telefono;
        entity.Ciudad = dto.Ciudad;
        entity.Direccion = dto.Direccion;
        entity.CodigoPostal = dto.CodigoPostal;
        entity.Bibliografia = dto.Bibliografia;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = dto.Estado;
    }

    public static OrganizacionDto ToDto(this Organizacion entity) => new()
    {
        IdOrganizacion = entity.IdOrganizacion,
        IdUsuarioCreador = entity.IdUsuarioCreador,
        IdPais = entity.IdPais,
        Nombre = entity.Nombre,
        Descripcion = entity.Descripcion,
        Direccion = entity.Direccion,
        Ciudad = entity.Ciudad,
        Email = entity.Email,
        Telefono = entity.Telefono,
        SitioWeb = entity.SitioWeb,
        FechaDesde = entity.FechaDesde,
        FechaHasta = entity.FechaHasta,
        Estado = entity.Estado
    };

    public static Organizacion ToEntity(this OrganizacionCreateDto dto) => new()
    {
        IdOrganizacion = Guid.NewGuid(),
        IdUsuarioCreador = dto.IdUsuarioCreador,
        IdPais = dto.IdPais,
        Nombre = dto.Nombre,
        Descripcion = dto.Descripcion,
        Direccion = dto.Direccion,
        Ciudad = dto.Ciudad,
        Email = dto.Email,
        Telefono = dto.Telefono,
        SitioWeb = dto.SitioWeb,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = dto.Estado
    };

    public static void UpdateFromDto(this Organizacion entity, OrganizacionUpdateDto dto)
    {
        entity.IdUsuarioCreador = dto.IdUsuarioCreador;
        entity.IdPais = dto.IdPais;
        entity.Nombre = dto.Nombre;
        entity.Descripcion = dto.Descripcion;
        entity.Direccion = dto.Direccion;
        entity.Ciudad = dto.Ciudad;
        entity.Email = dto.Email;
        entity.Telefono = dto.Telefono;
        entity.SitioWeb = dto.SitioWeb;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = dto.Estado;
    }

    public static RolDto ToDto(this Rol entity) => new()
    {
        IdRol = entity.IdRol,
        Nombre = entity.Nombre,
        Estado = entity.Estado
    };

    public static Rol ToEntity(this RolCreateDto dto) => new()
    {
        IdRol = dto.IdRol,
        Nombre = dto.Nombre,
        Estado = dto.Estado
    };

    public static void UpdateFromDto(this Rol entity, RolUpdateDto dto)
    {
        entity.Nombre = dto.Nombre;
        entity.Estado = dto.Estado;
    }

    public static RolUsuarioOrganizacionDto ToDto(this RolUsuarioOrganizacion entity) => new()
    {
        IdRolUsuarioOrganizacion = entity.IdRolUsuarioOrganizacion,
        IdOrganizacion = entity.IdOrganizacion,
        IdUsuarioAsignado = entity.IdUsuarioAsignado,
        IdUsuarioAdministrador = entity.IdUsuarioAdministrador,
        IdRol = entity.IdRol,
        FechaDesde = entity.FechaDesde,
        FechaHasta = entity.FechaHasta,
        Estado = entity.Estado
    };

    public static RolUsuarioOrganizacion ToEntity(this RolUsuarioOrganizacionCreateDto dto) => new()
    {
        IdRolUsuarioOrganizacion = Guid.NewGuid(),
        IdOrganizacion = dto.IdOrganizacion,
        IdUsuarioAsignado = dto.IdUsuarioAsignado,
        IdUsuarioAdministrador = dto.IdUsuarioAdministrador,
        IdRol = dto.IdRol,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = dto.Estado
    };

    public static void UpdateFromDto(this RolUsuarioOrganizacion entity, RolUsuarioOrganizacionUpdateDto dto)
    {
        entity.IdOrganizacion = dto.IdOrganizacion;
        entity.IdUsuarioAsignado = dto.IdUsuarioAsignado;
        entity.IdUsuarioAdministrador = dto.IdUsuarioAdministrador;
        entity.IdRol = dto.IdRol;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = dto.Estado;
    }

    public static CategoriaActividadDto ToDto(this CategoriaActividad entity) => new()
    {
        IdCategoriaActividad = entity.IdCategoriaActividad,
        Nombre = entity.Nombre,
        Estado = entity.Estado
    };

    public static CategoriaActividad ToEntity(this CategoriaActividadCreateDto dto) => new()
    {
        IdCategoriaActividad = dto.IdCategoriaActividad,
        Nombre = dto.Nombre,
        Estado = dto.Estado
    };

    public static void UpdateFromDto(this CategoriaActividad entity, CategoriaActividadUpdateDto dto)
    {
        entity.Nombre = dto.Nombre;
        entity.Estado = dto.Estado;
    }

    public static ActividadDto ToDto(this Actividad entity) => new()
    {
        IdActividad = entity.IdActividad,
        IdOrganizacion = entity.IdOrganizacion,
        IdUsuarioCreador = entity.IdUsuarioCreador,
        IdCategoria = entity.IdCategoria,
        IdPais = entity.IdPais,
        Nombre = entity.Nombre,
        Descripcion = entity.Descripcion,
        Lugar = entity.Lugar,
        FechaInicio = entity.FechaInicio,
        FechaFin = entity.FechaFin,
        Horas = entity.Horas,
        Cupos = entity.Cupos,
        Situacion = entity.Situacion,
        FechaDesde = entity.FechaDesde,
        FechaHasta = entity.FechaHasta,
        Estado = entity.Estado
    };

    public static Actividad ToEntity(this ActividadCreateDto dto) => new()
    {
        IdActividad = Guid.NewGuid(),
        IdOrganizacion = dto.IdOrganizacion,
        IdUsuarioCreador = dto.IdUsuarioCreador,
        IdCategoria = dto.IdCategoria,
        IdPais = dto.IdPais,
        Nombre = dto.Nombre,
        Descripcion = dto.Descripcion,
        Lugar = dto.Lugar,
        FechaInicio = dto.FechaInicio,
        FechaFin = dto.FechaFin,
        Horas = dto.Horas,
        Cupos = dto.Cupos,
        Situacion = dto.Situacion,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = dto.Estado
    };

    public static void UpdateFromDto(this Actividad entity, ActividadUpdateDto dto)
    {
        entity.IdOrganizacion = dto.IdOrganizacion;
        entity.IdUsuarioCreador = dto.IdUsuarioCreador;
        entity.IdCategoria = dto.IdCategoria;
        entity.IdPais = dto.IdPais;
        entity.Nombre = dto.Nombre;
        entity.Descripcion = dto.Descripcion;
        entity.Lugar = dto.Lugar;
        entity.FechaInicio = dto.FechaInicio;
        entity.FechaFin = dto.FechaFin;
        entity.Horas = dto.Horas;
        entity.Cupos = dto.Cupos;
        entity.Situacion = dto.Situacion;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = dto.Estado;
    }

    public static CoordinadorActividadDto ToDto(this CoordinadorActividad entity) => new()
    {
        IdCoordinadorActividad = entity.IdCoordinadorActividad,
        IdOrganizacion = entity.IdOrganizacion,
        IdActividad = entity.IdActividad,
        IdUsuarioCoordinador = entity.IdUsuarioCoordinador,
        FechaDesde = entity.FechaDesde,
        FechaHasta = entity.FechaHasta,
        Estado = entity.Estado
    };

    public static CoordinadorActividad ToEntity(this CoordinadorActividadCreateDto dto) => new()
    {
        IdCoordinadorActividad = Guid.NewGuid(),
        IdOrganizacion = dto.IdOrganizacion,
        IdActividad = dto.IdActividad,
        IdUsuarioCoordinador = dto.IdUsuarioCoordinador,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = dto.Estado
    };

    public static void UpdateFromDto(this CoordinadorActividad entity, CoordinadorActividadUpdateDto dto)
    {
        entity.IdOrganizacion = dto.IdOrganizacion;
        entity.IdActividad = dto.IdActividad;
        entity.IdUsuarioCoordinador = dto.IdUsuarioCoordinador;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = dto.Estado;
    }

    public static HorarioActividadDto ToDto(this HorarioActividad entity) => new()
    {
        IdHorarioActividad = entity.IdHorarioActividad,
        IdOrganizacion = entity.IdOrganizacion,
        IdActividad = entity.IdActividad,
        IdUsuario = entity.IdUsuario,
        Fecha = entity.Fecha,
        HoraInicio = entity.HoraInicio,
        HoraFin = entity.HoraFin,
        Descripcion = entity.Descripcion,
        FechaDesde = entity.FechaDesde,
        FechaHasta = entity.FechaHasta,
        Estado = entity.Estado
    };

    public static HorarioActividad ToEntity(this HorarioActividadCreateDto dto) => new()
    {
        IdHorarioActividad = Guid.NewGuid(),
        IdOrganizacion = dto.IdOrganizacion,
        IdActividad = dto.IdActividad,
        IdUsuario = dto.IdUsuario,
        Fecha = dto.Fecha,
        HoraInicio = dto.HoraInicio,
        HoraFin = dto.HoraFin,
        Descripcion = dto.Descripcion,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = dto.Estado
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
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = dto.Estado;
    }

    public static ParticipanteActividadDto ToDto(this ParticipanteActividad entity) => new()
    {
        IdParticipanteActividad = entity.IdParticipanteActividad,
        IdOrganizacion = entity.IdOrganizacion,
        IdActividad = entity.IdActividad,
        IdHorarioActividad = entity.IdHorarioActividad,
        IdUsuarioVoluntario = entity.IdUsuarioVoluntario,
        FechaInscripcion = entity.FechaInscripcion,
        FechaRetiro = entity.FechaRetiro,
        Situacion = entity.Situacion,
        FechaDesde = entity.FechaDesde,
        FechaHasta = entity.FechaHasta,
        Estado = entity.Estado
    };

    public static ParticipanteActividad ToEntity(this ParticipanteActividadCreateDto dto) => new()
    {
        IdParticipanteActividad = Guid.NewGuid(),
        IdOrganizacion = dto.IdOrganizacion,
        IdActividad = dto.IdActividad,
        IdHorarioActividad = dto.IdHorarioActividad,
        IdUsuarioVoluntario = dto.IdUsuarioVoluntario,
        FechaInscripcion = dto.FechaInscripcion,
        FechaRetiro = dto.FechaRetiro,
        Situacion = dto.Situacion,
        FechaDesde = dto.FechaDesde ?? DateTime.UtcNow,
        FechaHasta = dto.FechaHasta,
        Estado = dto.Estado
    };

    public static void UpdateFromDto(this ParticipanteActividad entity, ParticipanteActividadUpdateDto dto)
    {
        entity.IdOrganizacion = dto.IdOrganizacion;
        entity.IdActividad = dto.IdActividad;
        entity.IdHorarioActividad = dto.IdHorarioActividad;
        entity.IdUsuarioVoluntario = dto.IdUsuarioVoluntario;
        entity.FechaInscripcion = dto.FechaInscripcion;
        entity.FechaRetiro = dto.FechaRetiro;
        entity.Situacion = dto.Situacion;
        entity.FechaDesde = dto.FechaDesde ?? entity.FechaDesde;
        entity.FechaHasta = dto.FechaHasta;
        entity.Estado = dto.Estado;
    }

    public static CertificacionParticipacionDto ToDto(this CertificadoParticipacion entity) => new()
    {
        IdCertificacion = entity.IdCertificacion,
        IdParticipanteActividad = entity.IdParticipanteActividad,
        IdActividad = entity.IdActividad,
        IdOrganizacion = entity.IdOrganizacion,
        IdUsuarioVoluntario = entity.IdUsuarioVoluntario,
        FechaEmision = entity.FechaEmision,
        HorasTotales = entity.HorasTotales,
        DiasTotales = entity.DiasTotales,
        FechaPrimeraAsistencia = entity.FechaPrimeraAsistencia,
        FechaUltimaAsistencia = entity.FechaUltimaAsistencia,
        Situacion = entity.Situacion,
        Observaciones = entity.Observaciones,
        FechaRegistro = entity.FechaRegistro,
        FechaModificacion = entity.FechaModificacion,
        Estado = entity.Estado
    };

    public static CertificadoParticipacion ToEntity(this CertificacionParticipacionCreateDto dto) => new()
    {
        IdCertificacion = Guid.NewGuid(),
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
        FechaRegistro = dto.FechaRegistro ?? DateTime.UtcNow,
        FechaModificacion = dto.FechaModificacion,
        Estado = dto.Estado
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
        entity.FechaRegistro = dto.FechaRegistro ?? entity.FechaRegistro;
        entity.FechaModificacion = dto.FechaModificacion ?? DateTime.UtcNow;
        entity.Estado = dto.Estado;
    }
}
