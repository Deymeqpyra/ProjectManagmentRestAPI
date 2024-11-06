using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Projects.Exceptions;
using Application.Statuses.Exceptions;
using Domain.Projects;
using Domain.Statuses;
using Domain.Users;
using MediatR;

namespace Application.Projects.Commands;

public class SetStatusProjectCommand : IRequest<Result<Project, ProjectException>>
{
    public required Guid ProjectId { get; init; }
    public required Guid ProjectStatusId { get; init; }

    public required Guid UserId { get; init; }
}

public class
    SetStatusProjectCommandHandler(
        IProjectRepository repository,
        IStatusRepository statusRepository,
        IUserRepository userRepository)
    : IRequestHandler<SetStatusProjectCommand, Result<Project, ProjectException>>
{
    public async Task<Result<Project, ProjectException>> Handle(SetStatusProjectCommand request,
        CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var statusId = new ProjectStatusId(request.ProjectStatusId);
        var userId = new UserId(request.UserId);
        var exsitingProject = await repository.GetById(projectId, cancellationToken);
        var exisintgStatus = await statusRepository.GetById(statusId, cancellationToken);
        var existingUser = await userRepository.GetById(userId, cancellationToken);

        return await existingUser.Match(
            async u =>
            {
                return await exsitingProject.Match(
                    async p => await exisintgStatus.Match(
                        async s => await SetStatusEntity(p, u, s.Id, cancellationToken),
                        () => Task.FromResult<Result<Project, ProjectException>>(
                            new StatusNotFound(p.ProjectId, statusId))),
                    () => Task.FromResult<Result<Project, ProjectException>>
                        (new ProjectNotFound(projectId)));
            },
            () => Task.FromResult<Result<Project, ProjectException>>(new UserNotFoundWhileCreated(projectId)));
    }

    private async Task<Result<Project, ProjectException>> SetStatusEntity
    (Project project,
        User user,
        ProjectStatusId statusId,
        CancellationToken cancellationToken)
    {
        try
        {
            if (user.Id == project.UserId || user.Role.Name == "Admin")
            {
                project.SetStatus(statusId);

                return await repository.Update(project, cancellationToken);
            }

            return new NotEnoughPermission(project.ProjectId);
        }
        catch (Exception e)
        {
            return new ProjectUnknownException(project.ProjectId, e);
        }
    }
}