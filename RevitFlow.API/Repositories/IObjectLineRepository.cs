using RevitFlow.API.Models;

namespace RevitFlow.API.Repositories;

public interface IObjectLineRepository
{
    Task<IReadOnlyList<ObjectLine>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);

    Task BulkInsertAsync(int projectId, IReadOnlyList<ObjectLineCreateDto> items, CancellationToken cancellationToken = default);

    Task<int> DeleteByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);
}
