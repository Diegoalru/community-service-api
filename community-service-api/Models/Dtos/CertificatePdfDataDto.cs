namespace community_service_api.Models.Dtos;

/// <summary>
/// DTO que representa los datos devueltos por el procedimiento P_OBTENER_DATOS_PDF
/// para la generación del certificado PDF.
/// </summary>
public class CertificatePdfDataDto
{
    // Datos del Certificado
    public byte[] IdCertificacion { get; set; } = null!;
    public string FechaEmisionTexto { get; set; } = null!;
    public int HorasTotales { get; set; }
    public int DiasTotales { get; set; }
    public string FechaInicio { get; set; } = null!;
    public string FechaFin { get; set; } = null!;

    // Datos del Voluntario (Perfil)
    public string NombreVoluntario { get; set; } = null!;
    public string IdentificacionVoluntario { get; set; } = null!;
    public string? Carrera { get; set; }
    public string? UniversidadVoluntario { get; set; }

    // Datos de la Actividad y Organización
    public string NombreActividad { get; set; } = null!;
    public string NombreOrganizacion { get; set; } = null!;
    public string? DescripcionOrganizacion { get; set; }

    // Datos de Ubicación
    public string? LugarEvento { get; set; }

    /// <summary>
    /// Convierte el ID_CERTIFICACION (RAW) a Guid.
    /// </summary>
    public Guid GetIdCertificacionAsGuid()
    {
        return new Guid(IdCertificacion);
    }
}

