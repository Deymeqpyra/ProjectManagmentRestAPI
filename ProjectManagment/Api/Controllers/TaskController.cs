using Api.Dtos.TasksDto;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.ProjectTasks.Commands;
using Domain.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("tasks")]
[ApiController]
public class TaskController(ISender sender, ITaskQueries taskQueries) : ControllerBase
{
    [HttpGet("GetAll")]
    public async Task<ActionResult<IReadOnlyList<ProjectTaskDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entiteis = await taskQueries.GetAll(cancellationToken);

        return entiteis.Select(ProjectTaskDto.FromProjectTask).ToList();
    }

    [HttpGet("GetById/{taskId:guid}")]
    public async Task<ActionResult<ProjectTaskDto>> GetById([FromRoute] Guid taskId,
        CancellationToken cancellationToken)
    {
        var entity = await taskQueries.GetById(new ProjectTaskId(taskId), cancellationToken);

        return entity.Match<ActionResult<ProjectTaskDto>>(
            t => ProjectTaskDto.FromProjectTask(t),
            () => NotFound());
    }

    [HttpPost("Create")]
    public async Task<ActionResult<ProjectTaskDto>> Create([FromBody] CreateTaskDto projectTaskDto,
        CancellationToken cancellationToken)
    {
        var input = new CreateTaskForProjectCommand
        {
            TaskTitle = projectTaskDto.title,
            CategoryId = projectTaskDto.categoryId,
            ProjectId = projectTaskDto.projectId,
            ShortDescription = projectTaskDto.description
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProjectTaskDto>>(
            t => ProjectTaskDto.FromProjectTask(t),
            e => e.ToObjectResult());
    }

    [HttpPut("finishtask/{taskId:guid}")]
    public async Task<ActionResult<ProjectTaskDto>> FinishTask([FromRoute] Guid taskId,
        CancellationToken cancellationToken)
    {
        var input = new FinishTaskCommand
        {
            TaskId = taskId
        };
        
        var result = await sender.Send(input, cancellationToken);
        
        return result.Match<ActionResult<ProjectTaskDto>>(
            t=>ProjectTaskDto.FromProjectTask(t),
            e=>e.ToObjectResult());
    }

    [HttpPut("Update/{taskId:guid}")]
    public async Task<ActionResult<ProjectTaskDto>> Update([FromRoute] Guid taskId,[FromBody] ProjectTaskDto projectTaskDto,
        CancellationToken cancellationToken)
    {
        var input = new UpdateTaskCommand
        {
            TaskId = taskId,
            TitleUpdate = projectTaskDto.Title,
            DescriptionUpdate = projectTaskDto.Description,
            CategoryIdUpdate = projectTaskDto.CategoryId
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProjectTaskDto>>(
            t => ProjectTaskDto.FromProjectTask(t),
            e => e.ToObjectResult());
    }

    [HttpDelete("Delete/{taskId:guid}")]
    public async Task<ActionResult<ProjectTaskDto>> Delete([FromRoute] Guid taskId, CancellationToken cancellationToken)
    {
        var input = new DeleteTaskCommand
        {
            TaskId = taskId
        };
        
        var result = await sender.Send(input, cancellationToken);
        
        return result.Match<ActionResult<ProjectTaskDto>>(
            t=>ProjectTaskDto.FromProjectTask(t),
            e=>e.ToObjectResult());
    }
}