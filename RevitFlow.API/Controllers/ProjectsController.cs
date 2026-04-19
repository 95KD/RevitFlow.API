using Microsoft.AspNetCore.Mvc;
using RevitFlow.API.Models;
using RevitFlow.API.Repositories;

namespace RevitFlow.API.Controllers;

[ApiController]
[Route("projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;

    public ProjectsController(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProjectSettings>>> GetAll(CancellationToken cancellationToken)
    {
        var list = await _projectRepository.GetAllAsync(cancellationToken);
        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult<object>> Create([FromBody] CreateProjectRequest? request, CancellationToken cancellationToken)
    {
        request ??= new CreateProjectRequest();
        var id = await _projectRepository.CreateAsync(request.ProjectName, cancellationToken);
        return Created($"/projects/{id}", new { id });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _projectRepository.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
