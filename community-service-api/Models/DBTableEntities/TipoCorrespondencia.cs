using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

public partial class TipoCorrespondencia
{
    public int IdTipoCorrespondencia { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public virtual ICollection<Correspondencia> Correspondencia { get; set; } = new List<Correspondencia>();
}
