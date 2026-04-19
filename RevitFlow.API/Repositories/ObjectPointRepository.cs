using Dapper;
using RevitFlow.API.Data;
using RevitFlow.API.Models;

namespace RevitFlow.API.Repositories;

public class ObjectPointRepository : IObjectPointRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ObjectPointRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ObjectPoint>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT Id, ProjectId, ElementId, ObjectType, FamilyName, TypeName, LevelName,
                   X, Y, Z, FacingX, FacingY, FacingZ, HandX, HandY, HandZ, UpdatedAt
            FROM ObjectPoints
            WHERE ProjectId = @ProjectId
            ORDER BY Id
            """;

        await using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        var rows = await conn.QueryAsync<ObjectPoint>(new CommandDefinition(sql, new { ProjectId = projectId }, cancellationToken: cancellationToken));
        return rows.AsList();
    }

    public async Task BulkInsertAsync(int projectId, IReadOnlyList<ObjectPointCreateDto> items, CancellationToken cancellationToken = default)
    {
        if (items.Count == 0)
        {
            return;
        }

        const string sql = """
            INSERT INTO ObjectPoints (
                ProjectId, ElementId, ObjectType, FamilyName, TypeName, LevelName,
                X, Y, Z, FacingX, FacingY, FacingZ, HandX, HandY, HandZ, UpdatedAt)
            VALUES (
                @ProjectId, @ElementId, @ObjectType, @FamilyName, @TypeName, @LevelName,
                @X, @Y, @Z, @FacingX, @FacingY, @FacingZ, @HandX, @HandY, @HandZ, @UpdatedAt)
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
            i.X,
            i.Y,
            i.Z,
            i.FacingX,
            i.FacingY,
            i.FacingZ,
            i.HandX,
            i.HandY,
            i.HandZ,
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
        const string sql = "DELETE FROM ObjectPoints WHERE ProjectId = @ProjectId;";

        await using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        return await conn.ExecuteAsync(new CommandDefinition(sql, new { ProjectId = projectId }, cancellationToken: cancellationToken));
    }
}
