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
    public required Guid StatusId { get; init; }
    public required Guid PriorityId { get; init; }
    public required Guid UserId { get; init; }
}

public class CreateProjectCommandHandler(IProjectRepository repository, IUserRepository userRepository)
    : IRequestHandler<CreateProjectCommand, Result<Project, ProjectException>>
{
    public async Task<Result<Project, ProjectException>> Handle(CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        var exsitingProject = await repository.GetByTitle(request.Title, cancellationToken);
        var exsitingUser = await userRepository.GetById(new UserId(request.UserId), cancellationToken);
        return await exsitingUser.Match(
            async u =>
            {
                return await exsitingProject.Match(
                    p => Task.FromResult<Result<Project, ProjectException>>(new ProjectAlreadyExist(p.ProjectId)),
                    async () => await CreateEntity(
                        ProjectId.New(),
                        request.Title,
                        request.Description,
                        u.Id,
                        new ProjectStatusId(request.StatusId),
                        new ProjectPriorityId(request.PriorityId),
                        cancellationToken));
            },
            () => Task.FromResult<Result<Project, ProjectException>>(new UserNotFoundWhileCreated(ProjectId.Empty())));
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
            var entity = Project.New(projectId, title, desc, userId,  statusId, priorityId);

            return await repository.Create(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new ProjectUnknownException(ProjectId.Empty(), e);
        }
    }
}