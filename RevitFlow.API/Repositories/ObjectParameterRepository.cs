using Dapper;
using RevitFlow.API.Data;
using RevitFlow.API.Models;

namespace RevitFlow.API.Repositories;

public class ObjectParameterRepository : IObjectParameterRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ObjectParameterRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ObjectParameter>> GetByObjectAsync(string objectType, int objectId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT Id, ObjectType, ObjectId, ParamKey, ParamValue, UpdatedAt
            FROM ObjectParameters
            WHERE ObjectType = @ObjectType AND ObjectId = @ObjectId
            ORDER BY Id
            """;

        await using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        var rows = await conn.QueryAsync<ObjectParameter>(new CommandDefinition(sql, new { ObjectType = objectType, ObjectId = objectId }, cancellationToken: cancellationToken));
        return rows.AsList();
    }

    public async Task BulkInsertAsync(IReadOnlyList<ObjectParameterCreateDto> items, CancellationToken cancellationToken = default)
    {
        if (items.Count == 0)
        {
            return;
        }

        const string sql = """
            INSERT INTO ObjectParameters (ObjectType, ObjectId, ParamKey, ParamValue, UpdatedAt)
            VALUES (@ObjectType, @ObjectId, @ParamKey, @ParamValue, @UpdatedAt)
            """;

        var now = DateTime.UtcNow;
        var rows = items.Select(i => new
        {
            i.ObjectType,
            i.ObjectId,
            i.ParamKey,
            i.ParamValue,
            UpdatedAt = now
        }).ToList();

        await using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        await using var trx = await conn.BeginTransactionAsync(cancellationToken);
        try
        {
            await conn.ExecuteAsync(new CommandDefinition(sql, rows, transaction: trx, cancellationToken: cancellationToken));
            await trx.CommitAsync(cancellationToken);
        }
        catch
        {
            await trx.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<int> DeleteByObjectAsync(string objectType, int objectId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            DELETE FROM ObjectParameters
            WHERE ObjectType = @ObjectType AND ObjectId = @ObjectId
            """;

        await using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        return await conn.ExecuteAsync(new CommandDefinition(sql, new { ObjectType = objectType, ObjectId = objectId }, cancellationToken: cancellationToken));
    }
}
