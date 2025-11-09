using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

public partial class TipoControl
{
    public int IdTipoControl { get; set; }

    public string Nombre { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public virtual ICollection<ControlProceso> ControlProceso { get; set; } = new List<ControlProceso>();
}
