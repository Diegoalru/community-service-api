using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

public partial class Provincia
{
    public int IdPais { get; set; }

    public int IdProvincia { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Canton> Canton { get; set; } = new List<Canton>();

    public virtual Pais IdPaisNavigation { get; set; } = null!;
}
