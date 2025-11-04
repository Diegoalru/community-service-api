using System;
using System.Collections.Generic;

namespace community_service_api.Models.NewEntities;

public partial class Ubicacion
{
    public int IdUbicacion { get; set; }

    public int IdPais { get; set; }

    public int IdProvincia { get; set; }

    public int IdCanton { get; set; }

    public int IdDistrito { get; set; }

    public string Direccion { get; set; } = null!;

    public string? CodigoPostal { get; set; }

    public decimal? Latitud { get; set; }

    public decimal? Longitud { get; set; }

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Actividad> Actividad { get; set; } = new List<Actividad>();

    public virtual Distrito Distrito { get; set; } = null!;

    public virtual ICollection<Organizacion> Organizacion { get; set; } = new List<Organizacion>();

    public virtual ICollection<Perfil> Perfil { get; set; } = new List<Perfil>();
}
