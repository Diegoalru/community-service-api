using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

public partial class Universidad
{
    public int IdUniversidad { get; set; }

    public string Nombre { get; set; } = null!;

    public string Siglas { get; set; } = null!;

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Perfil> Perfil { get; set; } = new List<Perfil>();

    public virtual ICollection<Organizacion> Organizacion { get; set; } = new List<Organizacion>();
}

