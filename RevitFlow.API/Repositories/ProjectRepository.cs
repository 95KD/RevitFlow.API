using Dapper;
using RevitFlow.API.Data;
using RevitFlow.API.Models;

namespace RevitFlow.API.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ProjectRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ProjectSettings>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT Id, ProjectName, UpdatedAt
            FROM ProjectSettings
            ORDER BY Id
            """;

        await using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        var rows = await conn.QueryAsync<ProjectSettings>(new CommandDefinition(sql, cancellationToken: cancellationToken));
        return rows.AsList();
    }

    public async Task<int> CreateAsync(string? projectName, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO ProjectSettings (ProjectName, UpdatedAt)
            VALUES (@ProjectName, @UpdatedAt);
            SELECT LAST_INSERT_ID();
            """;

        var now = DateTime.UtcNow;
        await using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        var id = await conn.QuerySingleAsync<int>(new CommandDefinition(sql, new { ProjectName = projectName, UpdatedAt = now }, cancellationToken: cancellationToken));
        return id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        const string deleteLines = "DELETE FROM ObjectLines WHERE ProjectId = @Id;";
        const string deleteStyles = "DELETE FROM LineStyles WHERE ProjectId = @Id;";
        const string deletePoints = "DELETE FROM ObjectPoints WHERE ProjectId = @Id;";
        const string deleteProject = "DELETE FROM ProjectSettings WHERE Id = @Id;";

        await using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        await using var trx = await conn.BeginTransactionAsync(cancellationToken);
        try
        {
            await conn.ExecuteAsync(new CommandDefinition(deleteLines, new { Id = id }, transaction: trx, cancellationToken: cancellationToken));
            await conn.ExecuteAsync(new CommandDefinition(deleteStyles, new { Id = id }, transaction: trx, cancellationToken: cancellationToken));
            await conn.ExecuteAsync(new CommandDefinition(deletePoints, new { Id = id }, transaction: trx, cancellationToken: cancellationToken));
            var affected = await conn.ExecuteAsync(new CommandDefinition(deleteProject, new { Id = id }, transaction: trx, cancellationToken: cancellationToken));
            await trx.CommitAsync(cancellationToken);
            return affected > 0;
        }
        catch
        {
            await trx.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
