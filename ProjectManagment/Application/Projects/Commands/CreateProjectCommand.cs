using System.Globalization;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Projects.Exceptions;
using Domain.Priorities;
using Domain.Projects;
using Domain.Statuses;
using Domain.Users;
using MediatR;

namespace Application.Projects.Commands;

public class CreateProjectCommand : IRequest<Result<Project, ProjectException>>
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required Guid PriorityId { get; init; }
    public required Guid UserId { get; init; }
}

public class CreateProjectCommandHandler(
    IStatusRepository statusRepository,
    IProjectRepository repository,
    IUserRepository userRepository,
    IPriorityRepository priorityRepository)
    : IRequestHandler<CreateProjectCommand, Result<Project, ProjectException>>
{
    public async Task<Result<Project, ProjectException>> Handle(CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        var exsitingProject = await repository.GetByTitle(request.Title, cancellationToken);
        var exsitingUser = await userRepository.GetById(new UserId(request.UserId), cancellationToken);
        const string startedName = "Started";
        var exsitingStatus = await statusRepository.GetByName(startedName, cancellationToken);
        var exsitingPriority =
            await priorityRepository.GetById(new ProjectPriorityId(request.PriorityId), cancellationToken);

        return await exsitingStatus.Match(
            async status =>
            {
                return await exsitingPriority.Match(
                    async priority =>
                    {
                        return await exsitingUser.Match(
                            async u =>
                            {
                                return await exsitingProject.Match(
                                    p => Task.FromResult<Result<Project, ProjectException>>(
                                        new ProjectAlreadyExist(p.ProjectId)),
                                    async () => await CreateEntity(
                                        ProjectId.New(),
                                        request.Title,
                                        request.Description,
                                        u.Id,
                                        status.Id,
                                        priority.Id,
                                        cancellationToken));
                            },
                            () => Task.FromResult<Result<Project, ProjectException>>(
                                new UserNotFoundWhileCreated(ProjectId.Empty())));
                    },
                    () => Task.FromResult<Result<Project, ProjectException>>(
                        new PriorityNotFound(ProjectId.Empty(), new ProjectPriorityId(request.PriorityId))));
            },
            () => Task.FromResult<Result<Project, ProjectException>>(new StatusNotFound(ProjectId.Empty(),
                ProjectStatusId.New())));
    }

    private async Task<Result<Project, ProjectException>> CreateEntity
    (
        ProjectId projectId,
        string title,
        string desc,
        UserId userId,
        ProjectStatusId statusId,
        ProjectPriorityId priorityId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var entity = Project.New(projectId, title, desc, userId, statusId, priorityId);

            return await repository.Create(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new ProjectUnknownException(ProjectId.Empty(), e);
        }
    }
}