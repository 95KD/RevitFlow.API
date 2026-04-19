using Microsoft.AspNetCore.Mvc;
using RevitFlow.API.Models;
using RevitFlow.API.Repositories;

namespace RevitFlow.API.Controllers;

[ApiController]
[Route("projects/{projectId:int}/linestyles")]
public class LineStylesController : ControllerBase
{
    private readonly ILineStyleRepository _lineStyleRepository;

    public LineStylesController(ILineStyleRepository lineStyleRepository)
    {
        _lineStyleRepository = lineStyleRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LineStyle>>> GetByProject(int projectId, CancellationToken cancellationToken)
    {
        var list = await _lineStyleRepository.GetByProjectIdAsync(projectId, cancellationToken);
        return Ok(list);
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkInsert(int projectId, [FromBody] BulkLineStylesRequest? request, CancellationToken cancellationToken)
    {
        if (request?.Items is null)
        {
            return BadRequest("Items is required.");
        }

        await _lineStyleRepository.BulkInsertAsync(projectId, request.Items, cancellationToken);
        return NoContent();
    }
}
