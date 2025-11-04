using System;
using System.Collections.Generic;

namespace community_service_api.Models.NewEntities;

public partial class Canton
{
    public int IdPais { get; set; }

    public int IdProvincia { get; set; }

    public int IdCanton { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Distrito> Distrito { get; set; } = new List<Distrito>();

    public virtual Provincia Provincia { get; set; } = null!;
}
