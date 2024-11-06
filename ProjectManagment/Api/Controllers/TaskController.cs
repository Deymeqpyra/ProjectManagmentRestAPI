using Api.Dtos.TasksDto;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.ProjectTasks.Commands;
using Domain.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("tasks")]
[ApiController]
[Authorize]
public class TaskController(ISender sender, ITaskQueries taskQueries) : ControllerBase
{
    [Authorize(Roles = "Admin, User")]
    [HttpGet("GetAll")]
    public async Task<ActionResult<IReadOnlyList<ProjectTaskDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entiteis = await taskQueries.GetAll(cancellationToken);

        return entiteis.Select(ProjectTaskDto.FromProjectTask).ToList();
    }

    [Authorize(Roles = "Admin, User")]
    [HttpGet("GetById/{taskId:guid}")]
    public async Task<ActionResult<ProjectTaskDto>> GetById([FromRoute] Guid taskId,
        CancellationToken cancellationToken)
    {
        var entity = await taskQueries.GetById(new ProjectTaskId(taskId), cancellationToken);

        return entity.Match<ActionResult<ProjectTaskDto>>(
            t => ProjectTaskDto.FromProjectTask(t),
            () => NotFound());
    }

    [Authorize(Roles = "Admin, User")]
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

    [Authorize(Roles = "Admin, User")]
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
            t => ProjectTaskDto.FromProjectTask(t),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin, User")]
    [HttpPut("Update/{taskId:guid}")]
    public async Task<ActionResult<ProjectTaskDto>> Update([FromRoute] Guid taskId,
        [FromBody] ProjectTaskDto projectTaskDto,
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

    [Authorize(Roles = "Admin, User")]
    [HttpDelete("Delete/{taskId:guid}")]
    public async Task<ActionResult<ProjectTaskDto>> Delete([FromRoute] Guid taskId, CancellationToken cancellationToken)
    {
        var input = new DeleteTaskCommand
        {
            TaskId = taskId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProjectTaskDto>>(
            t => ProjectTaskDto.FromProjectTask(t),
            e => e.ToObjectResult());
    }
}