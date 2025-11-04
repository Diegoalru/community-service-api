using System;
using System.Collections.Generic;

namespace community_service_api.Models.NewEntities;

public partial class CoordinadorActividad
{
    public int IdCoordinadorActividad { get; set; }

    public int IdOrganizacion { get; set; }

    public int IdActividad { get; set; }

    public int IdUsuarioCoordinador { get; set; }

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Actividad Actividad { get; set; } = null!;

    public virtual Organizacion IdOrganizacionNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioCoordinadorNavigation { get; set; } = null!;
}
