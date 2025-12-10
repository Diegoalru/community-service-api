namespace community_service_api.Services;

public interface IIntegracionService
{
    Task<RespuestaRegistro> RegistrarUsuarioCompletoAsync(RegistroCompletoDto dto);
}

public class IntegracionService : IIntegracionService
{
    private readonly IUsuarioService _usuarioService;
    private readonly IPerfilService _perfilService;
    private readonly INotificacionService _notificacionService;

    public IntegracionService(
        IUsuarioService usuarioService,
        IPerfilService perfilService,
        INotificacionService notificacionService)
    {
        _usuarioService = usuarioService;
        _perfilService = perfilService;
        _notificacionService = notificacionService;
    }

    public async Task<RespuestaRegistro> RegistrarUsuarioCompletoAsync(RegistroCompletoDto dto)
    {
        // Serializar a JSON
        var jsonPayload = System.Text.Json.JsonSerializer.Serialize(dto);

        var dyParam = new OracleDynamicParameters();
        dyParam.Add("CV_DATOS", jsonPayload, OracleMappingType.Clob, ParameterDirection.Input);
        
        // Parámetros de Salida
        dyParam.Add("PN_ID_USUARIO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        dyParam.Add("PV_TOKEN", dbType: OracleMappingType.Varchar2, size: 2000, direction: ParameterDirection.Output);
        dyParam.Add("PB_EXITO", dbType: OracleMappingType.Byte, direction: ParameterDirection.Output);
        dyParam.Add("PV_MENSAJE", dbType: OracleMappingType.Varchar2, size: 2000, direction: ParameterDirection.Output);
        dyParam.Add("PN_CODIGO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

        await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_REGISTRO_USUARIO_JSON", dyParam);

        return new RespuestaRegistro
        {
            IdUsuario = dyParam.Get<int?>("PN_ID_USUARIO"),
            Token = dyParam.Get<string>("PV_TOKEN"),
            Mensaje = dyParam.Get<string>("PV_MENSAJE"),
            Codigo = dyParam.Get<int>("PN_CODIGO")
        };
    }
}

// Clases de respuesta auxiliares
public class RespuestaRegistro {
    public int Codigo { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public int? IdUsuario { get; set; }
    public string? Token { get; set; }
}

public class RespuestaReenvio {
    public int Codigo { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Token { get; set; }
}