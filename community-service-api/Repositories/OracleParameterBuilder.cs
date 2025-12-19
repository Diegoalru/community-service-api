using System.Data;
using Dapper.Oracle;

namespace community_service_api.Repositories;

/// <summary>
/// Helper para construir parámetros Oracle de forma fluida,
/// reduciendo la duplicación de código en los servicios de integración.
/// </summary>
public class OracleParameterBuilder
{
    private readonly OracleDynamicParameters _parameters = new();

    /// <summary>
    /// Crea una nueva instancia del builder.
    /// </summary>
    public static OracleParameterBuilder Create() => new();

    /// <summary>
    /// Agrega el parámetro de entrada PV_DATOS (CLOB) con el JSON payload.
    /// </summary>
    public OracleParameterBuilder WithJsonInput(string jsonPayload)
    {
        _parameters.Add("PV_DATOS", jsonPayload, OracleMappingType.Clob, ParameterDirection.Input);
        return this;
    }

    /// <summary>
    /// Agrega un parámetro de entrada personalizado.
    /// </summary>
    public OracleParameterBuilder WithInput<T>(string name, T value, OracleMappingType type)
    {
        _parameters.Add(name, value, type, ParameterDirection.Input);
        return this;
    }

    /// <summary>
    /// Agrega el parámetro de salida PN_ID_USUARIO (NUMBER).
    /// </summary>
    public OracleParameterBuilder WithOutputIdUsuario()
    {
        _parameters.Add("PN_ID_USUARIO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        return this;
    }

    /// <summary>
    /// Agrega el parámetro de salida PV_TOKEN (VARCHAR2).
    /// </summary>
    public OracleParameterBuilder WithOutputToken(int size = 2000)
    {
        _parameters.Add("PV_TOKEN", dbType: OracleMappingType.Varchar2, size: size, direction: ParameterDirection.Output);
        return this;
    }

    /// <summary>
    /// Agrega el parámetro de salida PV_EMAIL (VARCHAR2).
    /// </summary>
    public OracleParameterBuilder WithOutputEmail(int size = 2000)
    {
        _parameters.Add("PV_EMAIL", dbType: OracleMappingType.Varchar2, size: size, direction: ParameterDirection.Output);
        return this;
    }

    /// <summary>
    /// Agrega los parámetros de salida estándar: PN_EXITO, PV_MENSAJE, PN_CODIGO.
    /// </summary>
    public OracleParameterBuilder WithStandardOutputs()
    {
        _parameters.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        _parameters.Add("PV_MENSAJE", dbType: OracleMappingType.Varchar2, size: 2000, direction: ParameterDirection.Output);
        _parameters.Add("PN_CODIGO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        return this;
    }

    /// <summary>
    /// Agrega solo PN_EXITO y PV_MENSAJE (sin PN_CODIGO).
    /// </summary>
    public OracleParameterBuilder WithBasicOutputs()
    {
        _parameters.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        _parameters.Add("PV_MENSAJE", dbType: OracleMappingType.Varchar2, size: 2000, direction: ParameterDirection.Output);
        return this;
    }

    /// <summary>
    /// Agrega un parámetro de salida VARCHAR2 personalizado.
    /// </summary>
    public OracleParameterBuilder WithOutputVarchar(string name, int size = 2000)
    {
        _parameters.Add(name, dbType: OracleMappingType.Varchar2, size: size, direction: ParameterDirection.Output);
        return this;
    }

    /// <summary>
    /// Agrega un parámetro de salida NUMBER personalizado.
    /// </summary>
    public OracleParameterBuilder WithOutputNumber(string name)
    {
        _parameters.Add(name, dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        return this;
    }

    /// <summary>
    /// Agrega un parámetro de salida CLOB personalizado.
    /// </summary>
    public OracleParameterBuilder WithOutputClob(string name)
    {
        _parameters.Add(name, dbType: OracleMappingType.Clob, direction: ParameterDirection.Output);
        return this;
    }

    /// <summary>
    /// Construye y retorna los parámetros Oracle.
    /// </summary>
    public OracleDynamicParameters Build() => _parameters;

    /// <summary>
    /// Obtiene el valor de un parámetro de salida.
    /// </summary>
    public T? Get<T>(string name) => _parameters.Get<T>(name);
}

/// <summary>
/// Extensiones para extraer respuestas estándar de los parámetros Oracle.
/// </summary>
public static class OracleParameterExtensions
{
    /// <summary>
    /// Extrae una respuesta estándar de los parámetros de salida.
    /// </summary>
    public static (int Exito, string Mensaje, int Codigo) GetStandardResponse(this OracleDynamicParameters parameters)
    {
        return (
            parameters.Get<int>("PN_EXITO"),
            parameters.Get<string>("PV_MENSAJE") ?? string.Empty,
            parameters.Get<int>("PN_CODIGO")
        );
    }

    /// <summary>
    /// Extrae una respuesta básica (sin código) de los parámetros de salida.
    /// </summary>
    public static (int Exito, string Mensaje) GetBasicResponse(this OracleDynamicParameters parameters)
    {
        return (
            parameters.Get<int>("PN_EXITO"),
            parameters.Get<string>("PV_MENSAJE") ?? string.Empty
        );
    }
}

