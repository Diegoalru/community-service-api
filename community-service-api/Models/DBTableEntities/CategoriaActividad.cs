using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

public partial class CategoriaActividad
{
    public int IdCategoriaActividad { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Actividad> Actividad { get; set; } = new List<Actividad>();
}
