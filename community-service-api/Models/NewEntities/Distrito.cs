using System;
using System.Collections.Generic;

namespace community_service_api.Models.NewEntities;

public partial class Distrito
{
    public int IdPais { get; set; }

    public int IdProvincia { get; set; }

    public int IdCanton { get; set; }

    public int IdDistrito { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual Canton Canton { get; set; } = null!;

    public virtual ICollection<Ubicacion> Ubicacion { get; set; } = new List<Ubicacion>();
}
