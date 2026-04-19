using Microsoft.AspNetCore.Mvc;
using RevitFlow.API.Models;
using RevitFlow.API.Repositories;

namespace RevitFlow.API.Controllers;

[ApiController]
[Route("parameters")]
public class ObjectParametersController : ControllerBase
{
    private readonly IObjectParameterRepository _objectParameterRepository;

    public ObjectParametersController(IObjectParameterRepository objectParameterRepository)
    {
        _objectParameterRepository = objectParameterRepository;
    }

    [HttpGet("{objectType}/{objectId:int}")]
    public async Task<ActionResult<IReadOnlyList<ObjectParameter>>> GetByObject(string objectType, int objectId, CancellationToken cancellationToken)
    {
        var list = await _objectParameterRepository.GetByObjectAsync(objectType, objectId, cancellationToken);
        return Ok(list);
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkInsert([FromBody] BulkObjectParametersRequest? request, CancellationToken cancellationToken)
    {
        if (request?.Items is null)
        {
            return BadRequest("Items is required.");
        }

        await _objectParameterRepository.BulkInsertAsync(request.Items, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{objectType}/{objectId:int}")]
    public async Task<IActionResult> DeleteByObject(string objectType, int objectId, CancellationToken cancellationToken)
    {
        await _objectParameterRepository.DeleteByObjectAsync(objectType, objectId, cancellationToken);
        return NoContent();
    }
}
