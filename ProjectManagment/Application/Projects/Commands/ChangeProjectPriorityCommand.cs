using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Projects.Exceptions;
using Domain.Priorities;
using Domain.Projects;
using Domain.Users;
using MediatR;

namespace Application.Projects.Commands;

public class ChangeProjectPriorityCommand : IRequest<Result<Project, ProjectException>>
{
    public required Guid ProjectId { get; init; }
    public required Guid PriorityId { get; init; }
    public required Guid UserId { get; init; }
}

public class
    ChangeProjectPriorityCommandHandler(
        IProjectRepository repository,
        IPriorityRepository priorityRepository,
        IUserRepository userRepository)
    : IRequestHandler<ChangeProjectPriorityCommand,
        Result<Project, ProjectException>>
{
    public async Task<Result<Project, ProjectException>> Handle(ChangeProjectPriorityCommand request,
        CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var priorityId = new ProjectPriorityId(request.PriorityId);
        var userId = new UserId(request.UserId);
        var exsitingProject = await repository.GetById(projectId, cancellationToken);
        var exsitingPriority = await priorityRepository.GetById(priorityId, cancellationToken);
        var exsistingUser = await userRepository.GetById(userId, cancellationToken);

        return await exsistingUser.Match(
            async u =>
            {
                return await exsitingProject.Match(
                    async p =>
                    {
                        return await exsitingPriority.Match(
                            async pr => await ChangePriorityEntity(p, pr.Id, u, cancellationToken),
                            () => Task.FromResult<Result<Project, ProjectException>>(
                                new PriorityNotFound(projectId, priorityId)));
                    },
                    () => Task.FromResult<Result<Project, ProjectException>>(new ProjectNotFound(projectId)));
            },
            () => Task.FromResult<Result<Project, ProjectException>>(new UserNotFoundWhileCreated(projectId)));
    }

    private async Task<Result<Project, ProjectException>> ChangePriorityEntity(
        Project project,
        ProjectPriorityId priorityId,
        User user,
        CancellationToken cancellationToken)
    {
        try
        {
            if (user.Id == project.UserId || user.Role.Name == "Admin")
            {
                project.ChangePriority(priorityId);

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