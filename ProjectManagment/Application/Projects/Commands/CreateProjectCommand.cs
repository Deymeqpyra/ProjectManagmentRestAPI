using System.Globalization;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Projects.Exceptions;
using Domain.Priorities;
using Domain.Projects;
using Domain.Statuses;
using MediatR;

namespace Application.Projects.Commands;

public class CreateProjectCommand : IRequest<Result<Project, ProjectException>>
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public Guid StatusId { get; init; }
    public Guid PriorityId { get; init; }
}

public class CreateProjectCommandHandler(IProjectRepository repository)
    : IRequestHandler<CreateProjectCommand, Result<Project, ProjectException>>
{
    public async Task<Result<Project, ProjectException>> Handle(CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        var exsitingProject = await repository.GetByTitle(request.Title, cancellationToken);

        return await exsitingProject.Match(
            p => Task.FromResult<Result<Project, ProjectException>>(new ProjectAlreadyExist(p.ProjectId)),
            async () => await CreateEntity(
                ProjectId.New(),
                request.Title,
                request.Description,
                new ProjectStatusId(request.StatusId),
                new ProjectPriorityId(request.PriorityId),
                cancellationToken));
    }

    private async Task<Result<Project, ProjectException>> CreateEntity
    (
        ProjectId projectId,
        string title,
        string desc,
        ProjectStatusId statusId,
        ProjectPriorityId priorityId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var entity = Project.New(projectId, title, desc, statusId, priorityId);

            return await repository.Create(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new ProjectUnknownException(ProjectId.Empty(), e);
        }
    }
}