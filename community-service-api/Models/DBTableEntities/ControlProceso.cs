using System;

namespace community_service_api.Models.DBTableEntities;

public partial class ControlProceso
{
    public int IdControl { get; set; }

    public int TipoControl { get; set; }

    public DateTime InicioEjecucion { get; set; }

    public DateTime FinEjecucion { get; set; }

    public int? ActividadesProcesadas { get; set; }

    public int? CertificadosGenerados { get; set; }

    public int? CorreosEnviados { get; set; }

    public string Estado { get; set; } = null!;

    public string? ErrorMensaje { get; set; }

    public virtual TipoControl TipoControlNavigation { get; set; } = null!;
}
