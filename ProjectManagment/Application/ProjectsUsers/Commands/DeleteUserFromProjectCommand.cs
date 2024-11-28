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
    public required Guid UserIdWhoCreated { get; init; }
    public required Guid UserId { get; init; }
}

public class
    DeleteUserFromProjectCommandHandler(IProjectUserRepository repository,
        IProjectRepository projectRepository,
        IUserRepository userRepository)
    : IRequestHandler<DeleteUserFromProjectCommand,
        Result<ProjectUser, ProjectUserException>>
{
    public async Task<Result<ProjectUser, ProjectUserException>> Handle(DeleteUserFromProjectCommand request,
        CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var userId = new UserId(request.UserId);
        var userIdWhoCreated = new UserId(request.UserIdWhoCreated);
        var exsistingProjectUser = await repository.GetByIds(projectId, userId, cancellationToken);
        var existingProject = await projectRepository.GetById(projectId, cancellationToken);
        var existingUser = await userRepository.GetById(userIdWhoCreated, cancellationToken);

        return await existingProject.Match(
            async p =>
            {
                return await existingUser.Match(
                    async u =>
                    {
                        if (p.UserId == userIdWhoCreated || u.Role!.Name == "Admin")
                        {
                            return await exsistingProjectUser.Match(
                                async pu => await DeleteUser(pu, cancellationToken),
                                () => Task.FromResult<Result<ProjectUser, ProjectUserException>>(
                                    new ProjectUserNotFound(projectId, userId)));
                        }
                        return await Task.FromResult<Result<ProjectUser, ProjectUserException>>(
                            new UserNotEnoughPremission(projectId, userId));
                       
                    },
                    () => Task.FromResult<Result<ProjectUser, ProjectUserException>>(
                        new UserNotFound(projectId, userId)));
            },
            () => Task.FromResult<Result<ProjectUser, ProjectUserException>>(new ProjectNotFound(projectId, userId)));
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