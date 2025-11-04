using System.Data;
using Dapper.Oracle;

namespace community_service_api.Repositories;

public interface IProcedureRepository
{
    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<IEnumerable<T>> QueryAsync<T>(string procedureName, OracleDynamicParameters parameters);
    Task<T?> ExecuteAsync<T>(string procedureName, OracleDynamicParameters parameters, string outputParameterName);
}