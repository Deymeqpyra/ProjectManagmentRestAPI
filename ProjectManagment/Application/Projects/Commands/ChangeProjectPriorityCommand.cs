using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Projects.Exceptions;
using Domain.Priorities;
using Domain.Projects;
using MediatR;

namespace Application.Projects.Commands;

public class ChangeProjectPriorityCommand : IRequest<Result<Project, ProjectException>>
{
    public Guid ProjectId { get; init; }
    public Guid PriorityId { get; init; }
}

public class
    ChangeProjectPriorityCommandHandler(IProjectRepository repository, IPriorityRepository priorityRepository)
    : IRequestHandler<ChangeProjectPriorityCommand,
        Result<Project, ProjectException>>
{
    public async Task<Result<Project, ProjectException>> Handle(ChangeProjectPriorityCommand request,
        CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var priorityId = new ProjectPriorityId(request.PriorityId);
        var exsitingProject = await repository.GetById(projectId, cancellationToken);
        var exsitingPriority = await priorityRepository.GetById(priorityId, cancellationToken);

        return await exsitingProject.Match(
            async p =>
            {
                return await exsitingPriority.Match(
                    async pr => await ChangePriorityEntity(p, pr.Id, cancellationToken),
                    () => Task.FromResult<Result<Project, ProjectException>>(
                        new PriorityNotFound(projectId, priorityId)));
            },
            () => Task.FromResult<Result<Project, ProjectException>>(new ProjectNotFound(projectId)));
    }

    private async Task<Result<Project, ProjectException>> ChangePriorityEntity(
        Project project,
        ProjectPriorityId priorityId,
        CancellationToken cancellationToken)
    {
        try
        {
            project.ChangePriority(priorityId);

            return await repository.Update(project, cancellationToken);
        }
        catch (Exception e)
        {
            return new ProjectUnknownException(project.ProjectId, e);
        }
    }
}