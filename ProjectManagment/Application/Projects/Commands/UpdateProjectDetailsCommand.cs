using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Projects.Exceptions;
using Domain.Projects;
using Domain.Statuses;
using MediatR;

namespace Application.Projects.Commands;

public class UpdateProjectDetailsCommand : IRequest<Result<Project, ProjectException>>
{
    public required Guid ProjectId { get; init; }
    public required string UpdateTitle { get; init; }
    public required string UpdateDescription { get; init; }
}

public class UpdateProjectDetailsCommandHandler(IProjectRepository repository)
    : IRequestHandler<UpdateProjectDetailsCommand, Result<Project, ProjectException>>
{
    public async Task<Result<Project, ProjectException>> Handle(UpdateProjectDetailsCommand request,
        CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var exsitingProject = await repository.GetById(projectId, cancellationToken);

        return await exsitingProject.Match(
            async p => await UpdateEntity(p, request.UpdateTitle, request.UpdateDescription, cancellationToken),
            () => Task.FromResult<Result<Project, ProjectException>>(new ProjectNotFound(projectId))
        );
    }

    private async Task<Result<Project, ProjectException>> UpdateEntity(
        Project project,
        string updateTitle,
        string updateDescription,
        CancellationToken cancellationToken)
    {
        try
        {
            project.UpdateDetails(updateTitle, updateDescription);

            return await repository.Update(project, cancellationToken);
        }
        catch (Exception e)
        {
            return new ProjectUnknownException(project.ProjectId, e);
        }
    }
}