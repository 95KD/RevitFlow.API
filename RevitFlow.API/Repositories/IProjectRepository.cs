using RevitFlow.API.Models;

namespace RevitFlow.API.Repositories;

public interface IProjectRepository
{
    Task<IReadOnlyList<ProjectSettings>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<int> CreateAsync(string? projectName, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
