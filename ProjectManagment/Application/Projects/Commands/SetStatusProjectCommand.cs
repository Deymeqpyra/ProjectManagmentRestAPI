using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Projects.Exceptions;
using Application.Statuses.Exceptions;
using Domain.Projects;
using Domain.Statuses;
using MediatR;

namespace Application.Projects.Commands;

public class SetStatusProjectCommand : IRequest<Result<Project, ProjectException>>
{
    public required Guid ProjectId { get; init; }
    public required Guid ProjectStatusId { get; init; }
}

public class
    SetStatusProjectCommandHandler(IProjectRepository repository, IStatusRepository statusRepository)
    : IRequestHandler<SetStatusProjectCommand, Result<Project, ProjectException>>
{
    public async Task<Result<Project, ProjectException>> Handle(SetStatusProjectCommand request,
        CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var statusId = new ProjectStatusId(request.ProjectStatusId);
        var exsitingProject = await repository.GetById(projectId, cancellationToken);
        var exisintgStatus = await statusRepository.GetById(statusId, cancellationToken);
        return await exsitingProject.Match(
            async p => await exisintgStatus.Match(
                async s => await SetStatusEntity(p, s.Id, cancellationToken),
                () => Task.FromResult<Result<Project, ProjectException>>(new StatusNotFound(p.ProjectId, statusId))),
            () => Task.FromResult<Result<Project, ProjectException>>
                (new ProjectNotFound(projectId)));
    }

    private async Task<Result<Project, ProjectException>> SetStatusEntity
    (Project project,
        ProjectStatusId statusId,
        CancellationToken cancellationToken)
    {
        try
        {
            project.SetStatus(statusId);

            return await repository.Update(project, cancellationToken);
        }
        catch (Exception e)
        {
            return new ProjectUnknownException(project.ProjectId, e);
        }
    }
}