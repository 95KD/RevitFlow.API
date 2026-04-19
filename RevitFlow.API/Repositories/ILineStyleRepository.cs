using RevitFlow.API.Models;

namespace RevitFlow.API.Repositories;

public interface ILineStyleRepository
{
    Task<IReadOnlyList<LineStyle>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);

    Task BulkInsertAsync(int projectId, IReadOnlyList<LineStyleCreateDto> items, CancellationToken cancellationToken = default);
}
