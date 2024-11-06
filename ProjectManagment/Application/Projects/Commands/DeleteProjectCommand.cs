using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Projects.Exceptions;
using Domain.Projects;
using Domain.Users;
using MediatR;

namespace Application.Projects.Commands;

public class DeleteProjectCommand : IRequest<Result<Project, ProjectException>>
{
    public required Guid ProjectId { get; init; }
    public required Guid UserId { get; init; }
}

public class DeleteProjectCommandHandler(IProjectRepository repository, IUserRepository userRepository)
    : IRequestHandler<DeleteProjectCommand, Result<Project, ProjectException>>
{
    public async Task<Result<Project, ProjectException>> Handle(DeleteProjectCommand request,
        CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var userId = new UserId(request.UserId);
        var exisitingProject = await repository.GetById(projectId, cancellationToken);
        var exsistingUser = await userRepository.GetById(userId, cancellationToken);

        return await exsistingUser.Match(
            async u =>
            {
                return await exisitingProject.Match(
                    async p => await DeleteEntity(p, u, cancellationToken),
                    () => Task.FromResult<Result<Project, ProjectException>>(new ProjectNotFound(projectId)));
            },
            () => Task.FromResult<Result<Project, ProjectException>>(new UserNotFoundWhileCreated(projectId)));
    }

    private async Task<Result<Project, ProjectException>> DeleteEntity(
        Project project,
        User user,
        CancellationToken cancellationToken)
    {
        try
        {
            if (user.Id == project.UserId || user.Role.Name == "Admin")
            {
                return await repository.Delete(project, cancellationToken);
            }

            return new NotEnoughPermission(project.ProjectId);
        }
        catch (Exception e)
        {
            return new ProjectUnknownException(project.ProjectId, e);
        }
    }
}