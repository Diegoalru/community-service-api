using System.Data;
using System.Data.Common;
using community_service_api.DbContext;
using community_service_api.Exceptions;
using Dapper;
using Dapper.Oracle;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace community_service_api.Repositories;

/// <summary>
///     Repositorio encargado de ejecutar procedimientos almacenados y consultas en la base de datos Oracle.
///     Utiliza el <see cref="ApplicationDbContext" /> para obtener la conexión y Dapper/Dapper.Oracle para la ejecución.
///     Maneja errores de Oracle y los convierte en excepciones de dominio (<see cref="BusinessRuleException" />) o de
///     acceso a datos (<see cref="DataAccessException" />).
/// </summary>
public class ProcedureRepository(NewApplicationDbContext context) : IProcedureRepository
{
    
    private DbTransaction? _transaction;
    
    public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        var connection = context.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        if (connection is OracleConnection oc)
            oc.BindByName = true;

        _transaction = await connection.BeginTransactionAsync(isolationLevel);
    }
    
    public Task CommitTransactionAsync()
    {
        if (_transaction == null)
            return Task.CompletedTask;

        _transaction.Commit();
        _transaction.Dispose();
        _transaction = null;
        return Task.CompletedTask;
    }
    
    public Task RollbackTransactionAsync()
    {
        if (_transaction == null)
            return Task.CompletedTask;

        try
        {
            _transaction.Rollback();
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }

        return Task.CompletedTask;
    }
    
    /// <summary>
    ///     Ejecuta un procedimiento almacenado que devuelve un parámetro de salida.
    /// </summary>
    /// <param name="procedureName">Nombre del procedimiento almacenado a ejecutar (puede incluir el esquema).</param>
    /// <param name="parameters">Parámetros dinámicos de Dapper.Oracle que deben incluir el parámetro de salida.</param>
    /// <param name="outputParameterName">Nombre del parámetro de salida para obtener su valor después de la ejecución.</param>
    /// <returns>
    ///     El valor del parámetro de salida del procedimiento o <c>null</c> si el parámetro es nulo.
    /// </returns>
    /// <exception cref="BusinessRuleException">
    ///     Cuando Oracle lanza una excepción con número entre 20000 y 20999 (reglas de negocio definidas en PL/SQL).
    /// </exception>
    /// <exception cref="DataAccessException">
    ///     Para otras excepciones de Oracle que se mapean a mensajes más amigables.
    /// </exception>
    public async Task<T?> ExecuteAsync<T>(string procedureName,
        OracleDynamicParameters parameters,
        string outputParameterName)
    {
        var connection = context.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        if (connection is OracleConnection oc)
            oc.BindByName = true;

        try
        {   
            await connection.ExecuteAsync(
                procedureName,
                parameters,
                transaction: _transaction,
                commandType: CommandType.StoredProcedure
            );
            
            return parameters.Get<T?>(outputParameterName);
        }
        catch (OracleException ex) when (IsAppError(ex))
        {
            throw new BusinessRuleException(CleanOracleMessage(ex.Message), ex);
        }
        catch (OracleException ex)
        {
            throw new DataAccessException(MapOracleError(ex), ex);
        }
    }

    /// <summary>
    ///     Ejecuta un procedimiento almacenado que devuelve un conjunto de resultados y lo mapea a <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">Tipo del objeto de resultado esperado por cada fila del conjunto devuelto.</typeparam>
    /// <param name="procedureName">Nombre del procedimiento almacenado a ejecutar.</param>
    /// <param name="parameters">Parámetros dinámicos de Dapper.Oracle para la ejecución.</param>
    /// <returns>Una colección de objetos de tipo <typeparamref name="T" /> resultado del procedimiento.</returns>
    /// <exception cref="BusinessRuleException">
    ///     Cuando Oracle lanza una excepción con número entre 20000 y 20999 (reglas de negocio definidas en PL/SQL).
    /// </exception>
    /// <exception cref="DataAccessException">
    ///     Para otras excepciones de Oracle que se mapean a mensajes más amigables.
    /// </exception>
    public async Task<IEnumerable<T>> QueryAsync<T>(string procedureName, OracleDynamicParameters parameters)
    {
        var connection = context.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        if (connection is OracleConnection oc)
            oc.BindByName = true;

        try
        {
            return await connection.QueryAsync<T>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        catch (OracleException ex) when (IsAppError(ex))
        {
            throw new BusinessRuleException(CleanOracleMessage(ex.Message), ex);
        }
        catch (OracleException ex)
        {
            throw new DataAccessException(MapOracleError(ex), ex);
        }
    }

    /// <summary>
    ///     Asegura que la conexión a la base de datos esté abierta antes de usarla.
    /// </summary>
    /// <param name="connection">Conexión de base de datos.</param>
    private static async Task EnsureOpenAsync(DbConnection connection)
    {
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();
    }

    /// <summary>
    ///     Determina si una excepción de Oracle corresponde a un error de aplicación (regla de negocio).
    ///     Los errores de aplicación en Oracle se esperan en el rango 20000-20999.
    /// </summary>
    /// <param name="ex">La excepción de Oracle recibida.</param>
    /// <returns><c>true</c> si el número de errores está entre 20000 y 20999; en caso contrario <c>false</c>.</returns>
    private static bool IsAppError(OracleException ex)
    {
        return ex.Number is >= 20000 and <= 20999;
    }

    /// <summary>
    ///     Limpia el mensaje crudo de Oracle, removiendo el prefijo tipo "ORA-20xxx:" para dejar solo el texto legible.
    /// </summary>
    /// <param name="message">Mensaje original devuelto por Oracle.</param>
    /// <returns>Mensaje limpio y recortado.</returns>
    private static string CleanOracleMessage(string message)
    {
        // Quita el prefijo "ORA-20xxx: "
        var idx = message.IndexOf(':');
        return idx >= 0 ? message[(idx + 1)..].Trim() : message;
    }

    /// <summary>
    ///     Traduce algunos códigos de error comunes de Oracle a mensajes más amigables para la capa de acceso a datos.
    /// </summary>
    /// <param name="ex">Excepción de Oracle a mapear.</param>
    /// <returns>Mensaje mapeado o el mensaje original si no hay mapeo definido.</returns>
    private static string MapOracleError(OracleException ex)
    {
        return ex.Number switch
        {
            1 => "Violación de clave única (ORA-00001).",
            1400 => "Valor nulo en columna no anulable (ORA-01400).",
            2291 => "Restricción FK inexistente (ORA-02291).",
            2292 => "Restricción FK referenciada (ORA-02292).",
            _ => ex.Message
        };
    }
}