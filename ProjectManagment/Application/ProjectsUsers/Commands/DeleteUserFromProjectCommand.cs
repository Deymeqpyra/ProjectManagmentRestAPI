using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.ProjectsUsers.Exceptions;
using Domain.Projects;
using Domain.ProjectUsers;
using Domain.Users;
using MediatR;

namespace Application.ProjectsUsers.Commands;

public class DeleteUserFromProjectCommand : IRequest<Result<ProjectUser, ProjectUserException>>
{
    public required Guid ProjectId { get; init; }
    public required Guid UserId { get; init; }
}

public class
    DeleteUserFromProjectCommandHandler(IProjectUserRepository repository)
    : IRequestHandler<DeleteUserFromProjectCommand,
        Result<ProjectUser, ProjectUserException>>
{
    public async Task<Result<ProjectUser, ProjectUserException>> Handle(DeleteUserFromProjectCommand request,
        CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var userId = new UserId(request.UserId);
        var exsistingProjectUser = await repository.GetByIds(projectId, userId, cancellationToken);

        return await exsistingProjectUser.Match(
            async pu => await DeleteUser(pu, cancellationToken),
            () => Task.FromResult<Result<ProjectUser, ProjectUserException>>(
                new ProjectUserNotFound(projectId, userId)));
    }

    private async Task<Result<ProjectUser, ProjectUserException>> DeleteUser(ProjectUser projectUser,
        CancellationToken cancellationToken)
    {
        try
        {
            return await repository.Delete(projectUser, cancellationToken);
        }
        catch (Exception e)
        {
            return new ProjectUserUnknownException(projectUser.ProjectId, projectUser.UserId, e);
        }
    }
}