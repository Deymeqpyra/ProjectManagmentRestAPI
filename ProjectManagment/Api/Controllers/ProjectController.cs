using Api.Dtos.ProjectDto;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Projects.Commands;
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

    [HttpGet("getById/{projectId:guid}")]
    public async Task<ActionResult<ProjectDto>> GetProjectById(Guid projectId, CancellationToken cancellationToken)
    {
        var entity = await projectQueries.GetById(new ProjectId(projectId), cancellationToken);
        
        return entity.Match<ActionResult<ProjectDto>>(
            p=>ProjectDto.FromProject(p),
            () => NotFound());
    }

    [HttpPost("create")]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectDto dto, CancellationToken cancellationToken)
    {
        var input = new CreateProjectCommand
        {
            Title = dto.Title,
            Description = dto.Description,
            PriorityId = dto.priorityId,
            StatusId = dto.statusId
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProjectDto>>(
            p => ProjectDto.FromProject(p),
            e => e.ToObjectResult());
    }

    [HttpPut("update/{projectId:guid}")]
    public async Task<ActionResult<ProjectDto>> Update([FromRoute] Guid projectId, [FromBody] UpdateProjectDto dto,
        CancellationToken cancellationToken)
    {
        var input = new UpdateProjectDetailsCommand
        {
            ProjectId = projectId,
            UpdateTitle = dto.Title,
            UpdateDescription = dto.Description
        };

        var result = await sender.Send(input, cancellationToken);
        
        return result.Match<ActionResult<ProjectDto>>(
            p=>ProjectDto.FromProject(p),
            e=>e.ToObjectResult());
    }

    [HttpDelete("delete/{projectId:guid}")]
    public async Task<ActionResult<ProjectDto>> Delete([FromRoute] Guid projectId, CancellationToken cancellationToken)
    {
        var input = new DeleteProjectCommand
        {
            ProjectId = projectId
        };
        
        var result = await sender.Send(input, cancellationToken);
        
        return result.Match<ActionResult<ProjectDto>>(
            p=>ProjectDto.FromProject(p),
            e=>e.ToObjectResult());
    }

    [HttpPut("updateStatus/{projectId:guid}/status/{statusId:guid}")]
    public async Task<ActionResult<ProjectDto>> SetStatus([FromRoute] Guid projectId, [FromRoute] Guid statusId,
        CancellationToken cancellationToken)
    {
        var input = new SetStatusProjectCommand
        {
            ProjectId = projectId,
            ProjectStatusId = statusId
        };

        var result = await sender.Send(input, cancellationToken);
        
        return result.Match<ActionResult<ProjectDto>>
            (p=>ProjectDto.FromProject(p),
            e=>e.ToObjectResult());
    }
    
    [HttpPut("updatePriority/{projectId:guid}/priority/{priorityId:guid}")]
    public async Task<ActionResult<ProjectDto>> ChangePriority([FromRoute] Guid projectId, [FromRoute] Guid priorityId,
        CancellationToken cancellationToken)
    {
        var input = new ChangeProjectPriorityCommand
        {
            ProjectId = projectId,
            PriorityId = priorityId
        };

        var result = await sender.Send(input, cancellationToken);
        
        return result.Match<ActionResult<ProjectDto>>
            (p=>ProjectDto.FromProject(p),
            e=>e.ToObjectResult());
    }
}