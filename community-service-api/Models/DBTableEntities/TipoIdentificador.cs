using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

public partial class TipoIdentificador
{
    public int IdIdentificador { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Perfil> Perfil { get; set; } = new List<Perfil>();
}
