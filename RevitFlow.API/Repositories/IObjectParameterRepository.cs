using RevitFlow.API.Models;

namespace RevitFlow.API.Repositories;

public interface IObjectParameterRepository
{
    Task<IReadOnlyList<ObjectParameter>> GetByObjectAsync(string objectType, int objectId, CancellationToken cancellationToken = default);

    Task BulkInsertAsync(IReadOnlyList<ObjectParameterCreateDto> items, CancellationToken cancellationToken = default);

    Task<int> DeleteByObjectAsync(string objectType, int objectId, CancellationToken cancellationToken = default);
}
