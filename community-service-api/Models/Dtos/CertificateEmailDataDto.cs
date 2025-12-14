namespace community_service_api.Models.Dtos;

/// <summary>
/// DTO que representa los datos devueltos por el procedimiento P_OBTENER_DATOS_USUARIOS_ENVIO
/// para el envío de correos con certificados.
/// </summary>
public class CertificateEmailDataDto
{
    public required byte[] IdCertificacion { get; set; }
    public string NombreVoluntario { get; set; } = null!;
    public string EmailVoluntario { get; set; } = null!;
    public byte[]? Documento { get; set; }

    public Guid GetIdCertificacionAsGuid()
    {
        return new Guid(IdCertificacion);
    }
}

