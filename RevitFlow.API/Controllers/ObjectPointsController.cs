using Microsoft.AspNetCore.Mvc;
using RevitFlow.API.Models;
using RevitFlow.API.Repositories;

namespace RevitFlow.API.Controllers;

[ApiController]
[Route("projects/{projectId:int}/points")]
public class ObjectPointsController : ControllerBase
{
    private readonly IObjectPointRepository _objectPointRepository;

    public ObjectPointsController(IObjectPointRepository objectPointRepository)
    {
        _objectPointRepository = objectPointRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ObjectPoint>>> GetByProject(int projectId, CancellationToken cancellationToken)
    {
        var list = await _objectPointRepository.GetByProjectIdAsync(projectId, cancellationToken);
        return Ok(list);
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkInsert(int projectId, [FromBody] BulkObjectPointsRequest? request, CancellationToken cancellationToken)
    {
        if (request?.Items is null)
        {
            return BadRequest("Items is required.");
        }

        await _objectPointRepository.BulkInsertAsync(projectId, request.Items, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteByProject(int projectId, CancellationToken cancellationToken)
    {
        await _objectPointRepository.DeleteByProjectIdAsync(projectId, cancellationToken);
        return NoContent();
    }
}
