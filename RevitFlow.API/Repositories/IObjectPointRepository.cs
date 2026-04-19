using RevitFlow.API.Models;

namespace RevitFlow.API.Repositories;

public interface IObjectPointRepository
{
    Task<IReadOnlyList<ObjectPoint>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);

    Task BulkInsertAsync(int projectId, IReadOnlyList<ObjectPointCreateDto> items, CancellationToken cancellationToken = default);

    Task<int> DeleteByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);
}
