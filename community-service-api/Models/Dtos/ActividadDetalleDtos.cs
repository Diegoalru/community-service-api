using System;

namespace community_service_api.Models.Dtos;

public class ActividadDetalleDto
{
    public int IdActividad { get; set; }
    public int IdOrganizacion { get; set; }
    public int IdUsuarioCreador { get; set; }
    public int IdCategoria { get; set; }
    public int IdUbicacion { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int? Horas { get; set; }
    public int Cupos { get; set; }
    public char Situacion { get; set; }
    public char Estado { get; set; }

    public OrganizacionBasicaDto Organizacion { get; set; } = new();
    public UsuarioBasicoDto UsuarioCreador { get; set; } = new();
    public CategoriaActividadBasicaDto Categoria { get; set; } = new();
    public UbicacionBasicaDto Ubicacion { get; set; } = new();
    public List<HorarioActividadBasicoDto> Horarios { get; set; } = new();

    public bool UsuarioInscrito { get; set; }
}

public class OrganizacionBasicaDto
{
    public int IdOrganizacion { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class UsuarioBasicoDto
{
    public int IdUsuario { get; set; }
    public string Username { get; set; } = string.Empty;
}

public class CategoriaActividadBasicaDto
{
    public int IdCategoriaActividad { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class UbicacionBasicaDto
{
    public int IdUbicacion { get; set; }
    public int IdPais { get; set; }
    public int? IdProvincia { get; set; }
    public int? IdCanton { get; set; }
    public int? IdDistrito { get; set; }
    public string? Direccion { get; set; }
    public string? CodigoPostal { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public char Estado { get; set; }
}

public class HorarioActividadBasicoDto
{
    public int IdHorarioActividad { get; set; }
    public int IdOrganizacion { get; set; }
    public int IdActividad { get; set; }
    public DateTime Fecha { get; set; }
    public DateTime HoraInicio { get; set; }
    public DateTime HoraFin { get; set; }
    public string? Descripcion { get; set; }
    public char Situacion { get; set; }
    public char Estado { get; set; }
}



