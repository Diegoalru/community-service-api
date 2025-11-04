using System;
using System.Collections.Generic;

namespace community_service_api.Models.NewEntities;

public partial class RolUsuarioOrganizacion
{
    public int IdRolUsuarioOrganizacion { get; set; }

    public int IdOrganizacion { get; set; }

    public int IdUsuarioAsignado { get; set; }

    public int IdUsuarioAdministrador { get; set; }

    public int IdRol { get; set; }

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Organizacion IdOrganizacionNavigation { get; set; } = null!;

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioAdministradorNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioAsignadoNavigation { get; set; } = null!;
}
