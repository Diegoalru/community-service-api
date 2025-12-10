using System;

namespace community_service_api.Models.DBTableEntities;

public partial class Perfil
{
    public int IdPerfil { get; set; }

    public int IdUsuario { get; set; }

    public int IdUbicacion { get; set; }

    public int IdIdentificador { get; set; }

    public int? IdUniversidad { get; set; }

    public string Identificacion { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string ApellidoP { get; set; } = null!;

    public string? ApellidoM { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public string? Carrera { get; set; }

    public string? Bibliografia { get; set; }

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual TipoIdentificador IdIdentificadorNavigation { get; set; } = null!;

    public virtual Ubicacion IdUbicacionNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual Universidad? IdUniversidadNavigation { get; set; }
}
