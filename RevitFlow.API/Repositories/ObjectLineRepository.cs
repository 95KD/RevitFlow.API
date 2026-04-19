using Dapper;
using RevitFlow.API.Data;
using RevitFlow.API.Models;

namespace RevitFlow.API.Repositories;

public class ObjectLineRepository : IObjectLineRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ObjectLineRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ObjectLine>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT Id, ProjectId, ElementId, ObjectType, FamilyName, TypeName, LevelName,
                   StartX, StartY, StartZ, EndX, EndY, EndZ, LineStyleId, UpdatedAt
            FROM ObjectLines
            WHERE ProjectId = @ProjectId
            ORDER BY Id
            """;

        await using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        var rows = await conn.QueryAsync<ObjectLine>(new CommandDefinition(sql, new { ProjectId = projectId }, cancellationToken: cancellationToken));
        return rows.AsList();
    }

    public async Task BulkInsertAsync(int projectId, IReadOnlyList<ObjectLineCreateDto> items, CancellationToken cancellationToken = default)
    {
        if (items.Count == 0)
        {
            return;
        }

        const string sql = """
            INSERT INTO ObjectLines (
                ProjectId, ElementId, ObjectType, FamilyName, TypeName, LevelName,
                StartX, StartY, StartZ, EndX, EndY, EndZ, LineStyleId, UpdatedAt)
            VALUES (
                @ProjectId, @ElementId, @ObjectType, @FamilyName, @TypeName, @LevelName,
                @StartX, @StartY, @StartZ, @EndX, @EndY, @EndZ, @LineStyleId, @UpdatedAt)
            """;

        var now = DateTime.UtcNow;
        var rows = items.Select(i => new
        {
            ProjectId = projectId,
            i.ElementId,
            i.ObjectType,
            i.FamilyName,
            i.TypeName,
            i.LevelName,
            i.StartX,
            i.StartY,
            i.StartZ,
            i.EndX,
            i.EndY,
            i.EndZ,
            i.LineStyleId,
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

    public async Task<int> DeleteByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM ObjectLines WHERE ProjectId = @ProjectId;";

        await using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        return await conn.ExecuteAsync(new CommandDefinition(sql, new { ProjectId = projectId }, cancellationToken: cancellationToken));
    }
}
