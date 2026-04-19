using Dapper;
using RevitFlow.API.Data;
using RevitFlow.API.Models;

namespace RevitFlow.API.Repositories;

public class LineStyleRepository : ILineStyleRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public LineStyleRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<LineStyle>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT Id, ProjectId, StyleName, Color, Weight, Pattern
            FROM LineStyles
            WHERE ProjectId = @ProjectId
            ORDER BY Id
            """;

        await using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        var rows = await conn.QueryAsync<LineStyle>(new CommandDefinition(sql, new { ProjectId = projectId }, cancellationToken: cancellationToken));
        return rows.AsList();
    }

    public async Task BulkInsertAsync(int projectId, IReadOnlyList<LineStyleCreateDto> items, CancellationToken cancellationToken = default)
    {
        if (items.Count == 0)
        {
            return;
        }

        const string sql = """
            INSERT INTO LineStyles (ProjectId, StyleName, Color, Weight, Pattern)
            VALUES (@ProjectId, @StyleName, @Color, @Weight, @Pattern)
            """;

        var rows = items.Select(i => new
        {
            ProjectId = projectId,
            i.StyleName,
            i.Color,
            i.Weight,
            i.Pattern
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
}
