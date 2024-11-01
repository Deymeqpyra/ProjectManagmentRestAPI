using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Projects.Exceptions;
using Domain.Projects;
using MediatR;

namespace Application.Projects.Commands;

public class AddComentToProjectCommand : IRequest<Result<Project, ProjectException>>
{
    public required Guid ProjectId { get; init; }
    public string CommentMessage { get; init; }
}
public class AddCommentToProjectCommandHandler(IProjectRepository repository) : IRequestHandler<AddComentToProjectCommand, Result<Project, ProjectException>>
{
    public async Task<Result<Project, ProjectException>> Handle(AddComentToProjectCommand request, CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var existingProject = await repository.GetById(projectId, cancellationToken);

        return await existingProject.Match(
            async p => await AddComment(p, request.CommentMessage, cancellationToken),
            () => Task.FromResult<Result<Project, ProjectException>>(new ProjectNotFound(projectId)));
    }

    private async Task<Result<Project, ProjectException>> AddComment(
        Project project,
        string comment,
        CancellationToken cancellationToken)
    {
        try
        {
            project.AddComment(comment);
            
            return await repository.Update(project, cancellationToken);
        }
        catch (Exception e)
        {
            return new ProjectUnknownException(project.ProjectId, e);
        }
    }
}