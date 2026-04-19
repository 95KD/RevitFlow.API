using Microsoft.AspNetCore.Mvc;
using RevitFlow.API.Models;
using RevitFlow.API.Repositories;

namespace RevitFlow.API.Controllers;

[ApiController]
[Route("projects/{projectId:int}/lines")]
public class ObjectLinesController : ControllerBase
{
    private readonly IObjectLineRepository _objectLineRepository;

    public ObjectLinesController(IObjectLineRepository objectLineRepository)
    {
        _objectLineRepository = objectLineRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ObjectLine>>> GetByProject(int projectId, CancellationToken cancellationToken)
    {
        var list = await _objectLineRepository.GetByProjectIdAsync(projectId, cancellationToken);
        return Ok(list);
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkInsert(int projectId, [FromBody] BulkObjectLinesRequest? request, CancellationToken cancellationToken)
    {
        if (request?.Items is null)
        {
            return BadRequest("Items is required.");
        }

        await _objectLineRepository.BulkInsertAsync(projectId, request.Items, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteByProject(int projectId, CancellationToken cancellationToken)
    {
        await _objectLineRepository.DeleteByProjectIdAsync(projectId, cancellationToken);
        return NoContent();
    }
}
