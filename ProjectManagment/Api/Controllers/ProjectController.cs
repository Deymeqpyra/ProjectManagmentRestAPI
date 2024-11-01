using Api.Dtos.ProjectDto;
using Application.Common.Interfaces.Queries;
using Domain.Projects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("projects")]
[ApiController]
public class ProjectController(ISender sender, IProjectQueries projectQueries) : ControllerBase
{
    [HttpGet("getAll")]
    public async Task<ActionResult<IReadOnlyList<ProjectDto>>> GetProjects(CancellationToken cancellationToken)
    {
        var entities = await projectQueries.GetAll(cancellationToken);
        
        return entities.Select(ProjectDto.FromProject).ToList();
    }
}