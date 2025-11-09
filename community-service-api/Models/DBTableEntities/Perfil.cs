using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

public partial class Perfil
{
    public int IdPerfil { get; set; }

    public int IdUsuario { get; set; }

    public int IdIdentificador { get; set; }

    public int IdPais { get; set; }

    public int IdUbicacion { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoP { get; set; } = null!;

    public string ApellidoM { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string? Direccion { get; set; }

    public string? Bibliografia { get; set; }

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual TipoIdentificador IdIdentificadorNavigation { get; set; } = null!;

    public virtual Pais IdPaisNavigation { get; set; } = null!;

    public virtual Ubicacion IdUbicacionNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
