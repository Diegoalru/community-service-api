using System;
using System.Collections.Generic;

namespace community_service_api.Models.NewEntities;

public partial class Correspondencia
{
    public int IdCorrespondencia { get; set; }

    public int IdUsuario { get; set; }

    public int IdTipoCorrespondencia { get; set; }

    public string Valor { get; set; } = null!;

    public string Consentimiento { get; set; } = null!;

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual TipoCorrespondencia IdTipoCorrespondenciaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
