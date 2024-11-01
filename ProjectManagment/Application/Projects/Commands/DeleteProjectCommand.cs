using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Projects.Exceptions;
using Domain.Projects;
using MediatR;

namespace Application.Projects.Commands;

public class DeleteProjectCommand : IRequest<Result<Project, ProjectException>>
{
    public required Guid ProjectId { get; init; }
}
public class DeleteProjectCommandHandler(IProjectRepository repository) : IRequestHandler<DeleteProjectCommand, Result<Project, ProjectException>>
{
    public async Task<Result<Project, ProjectException>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var exisitingProject = await repository.GetById(projectId, cancellationToken);

        return await exisitingProject.Match(
            async p => await DeleteEntity(p, cancellationToken),
            () => Task.FromResult<Result<Project, ProjectException>>(new ProjectNotFound(projectId)));
    }

    private async Task<Result<Project, ProjectException>> DeleteEntity(Project project,
        CancellationToken cancellationToken)
    {
        try
        {
            return await repository.Delete(project, cancellationToken);
        }
        catch (Exception e)
        {
            return new ProjectUnknownException(project.ProjectId, e);
        }
    }
}