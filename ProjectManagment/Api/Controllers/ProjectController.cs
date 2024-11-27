using System.Security.Claims;
using Api.Dtos.CommentsDto;
using Api.Dtos.ProjectDto;
using Api.Dtos.ProjectsUsersDto;
using Api.Dtos.TagsProjects;
using Api.Modules.Errors;
using Application.Comments.Commands;
using Application.Common.Interfaces.Queries;
using Application.Projects.Commands;
using Application.ProjectsUsers.Commands;
using Application.TagsProjects.Commands;
using Domain.Projects;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("projects")]
[ApiController]
[Authorize]
public class ProjectController(ISender sender, IProjectQueries projectQueries) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet("getAll")]
    public async Task<ActionResult<IReadOnlyList<ProjectDto>>> GetProjects(CancellationToken cancellationToken)
    {
        var entities = await projectQueries.GetAll(cancellationToken);

        return entities.Select(ProjectDto.FromProject).ToList();
    }
    [Authorize(Roles = "Admin, User")]
    [HttpGet("getAllByUser")]
    public async Task<ActionResult<IReadOnlyList<ProjectDto>>> GetProjectsByUserId(CancellationToken cancellationToken)
    {
        string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdClaim);
        var entities = await projectQueries.GetByUserId(new UserId(userId), cancellationToken);

        return entities.Select(ProjectDto.FromProject).ToList();
    }

    [Authorize(Roles="Admin, User")]
    [HttpGet("getById/{projectId:guid}")]
    public async Task<ActionResult<ProjectDto>> GetProjectById(Guid projectId, CancellationToken cancellationToken)
    {
        var entity = await projectQueries.GetById(new ProjectId(projectId), cancellationToken);

        return entity.Match<ActionResult<ProjectDto>>(
            p => ProjectDto.FromProject(p),
            () => NotFound());
    }

    [Authorize(Roles = "Admin, User")]
    [HttpPost("create")]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectDto dto,
        CancellationToken cancellationToken)
    {
        string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdClaim);
        var input = new CreateProjectCommand
        {
            Title = dto.Title,
            Description = dto.Description,
            PriorityId = dto.priorityId,
            StatusId = dto.statusId,
            UserId = userId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProjectDto>>(
            p => ProjectDto.FromProject(p),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin, User")]
    [HttpPost("AddTag/{tagId:guid}/toProject/{projectId:guid}")]
    public async Task<ActionResult<TagProjectDto>> AddTag([FromRoute] Guid tagId, [FromRoute] Guid projectId,
        CancellationToken cancellationToken)
    {
        string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdClaim);
        var input = new AddTagForProjectCommand
        {
            ProjectId = projectId,
            TagId = tagId,
            UserId = userId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<TagProjectDto>>(
            p => TagProjectDto.FromProject(p),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin, User")]
    [HttpPost("addComment/{projectId:guid}")]
    public async Task<ActionResult<CommentDto>> AddComment(
        [FromRoute] Guid projectId,
        [FromBody] CreateCommentDto comment,
        CancellationToken cancellationToken)
    {
        string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdClaim);
        var input = new AddComentToProjectCommand
        {
            ProjectId = projectId,
            CommentMessage = comment.text,
            UserId = userId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CommentDto>>(
            p => CommentDto.FromDomain(p),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin, User")]
    [HttpPut("addUser/{userId:guid}/toProject/{projectId:guid}")]
    public async Task<ActionResult<ProjectUsersDto>> AddUserToProjet(
        [FromRoute] Guid projectId,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userIdWhoCreated = Guid.Parse(userIdClaim);
        var input = new AddUserToProjectCommand
        {
            ProjectId = projectId,
            UserId = userId,
            UserIdWhoCreated = userIdWhoCreated
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProjectUsersDto>>(
            p => ProjectUsersDto.FromProject(p),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin, User")]
    [HttpDelete("deleteUser/{userId:guid}/fromProject/{projectId:guid}")]
    public async Task<ActionResult<ProjectUsersDto>> DeleteUserFromProjet(
        [FromRoute] Guid projectId,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userIdWhoCreated = Guid.Parse(userIdClaim);
        var input = new DeleteUserFromProjectCommand
        {
            ProjectId = projectId,
            UserId = userId,
            UserIdWhoCreated = userIdWhoCreated
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProjectUsersDto>>(
            p => ProjectUsersDto.FromProject(p),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin, User")]
    [HttpDelete("Delete/{tagId:guid}/inProject/{projectId:guid}")]
    public async Task<ActionResult<TagProjectDto>> DeleteTag([FromRoute] Guid tagId, [FromRoute] Guid projectId,
        CancellationToken cancellationToken)
    {
        string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdClaim);
        var input = new DeleteTagFromProjectCommand
        {
            ProjectId = projectId,
            TagId = tagId,
            UserId = userId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<TagProjectDto>>(
            p => TagProjectDto.FromProject(p),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin, User")]
    [HttpPut("update/{projectId:guid}")]
    public async Task<ActionResult<ProjectDto>> Update([FromRoute] Guid projectId, [FromBody] UpdateProjectDto dto,
        CancellationToken cancellationToken)
    {
        string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdClaim);
        var input = new UpdateProjectDetailsCommand
        {
            ProjectId = projectId,
            UpdateTitle = dto.Title,
            UpdateDescription = dto.Description,
            UserId = userId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProjectDto>>(
            p => ProjectDto.FromProject(p),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin, User")]
    [HttpDelete("deleteProject/{projectId:guid}")]
    public async Task<ActionResult<ProjectDto>> Delete([FromRoute] Guid projectId, CancellationToken cancellationToken)
    {
        string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdClaim);
        var input = new DeleteProjectCommand
        {
            ProjectId = projectId,
            UserId = userId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProjectDto>>(
            p => ProjectDto.FromProject(p),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin, User")]
    [HttpPut("updateStatus/{projectId:guid}/status/{statusId:guid}")]
    public async Task<ActionResult<ProjectDto>> SetStatus([FromRoute] Guid projectId, [FromRoute] Guid statusId,
        CancellationToken cancellationToken)
    {
        string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdClaim);
        var input = new SetStatusProjectCommand
        {
            ProjectId = projectId,
            ProjectStatusId = statusId,
            UserId = userId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProjectDto>>
        (p => ProjectDto.FromProject(p),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin, User")]
    [HttpPut("updatePriority/{projectId:guid}/priority/{priorityId:guid}")]
    public async Task<ActionResult<ProjectDto>> ChangePriority([FromRoute] Guid projectId, [FromRoute] Guid priorityId,
        CancellationToken cancellationToken)
    {
        string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdClaim);
        var input = new ChangeProjectPriorityCommand
        {
            ProjectId = projectId,
            PriorityId = priorityId,
            UserId = userId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProjectDto>>
        (p => ProjectDto.FromProject(p),
            e => e.ToObjectResult());
    }
}