using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.TagsProjects.Exceptions;
using Domain.Projects;
using Domain.Tags;
using Domain.TagsProjects;
using MediatR;

namespace Application.TagsProjects.Commands;

public class DeleteTagFromProjectCommand : IRequest<Result<TagsProject, TagProjectException>>
{
    public required Guid ProjectId { get; init; }
    public required Guid TagId { get; init; }
}

public class DeleteTagFromProjectCommandHandler(ITagProjectRepository repository)
    : IRequestHandler<DeleteTagFromProjectCommand, Result<TagsProject, TagProjectException>>
{
    public async Task<Result<TagsProject, TagProjectException>> Handle(DeleteTagFromProjectCommand request,
        CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var tagId = new TagId(request.TagId);
        var existingTagProject = await repository.GetByTagAndProjectId(tagId, projectId, cancellationToken);

        return await existingTagProject.Match(
            async tg => await DeleteEntity(tg, cancellationToken),
            () => Task.FromResult<Result<TagsProject, TagProjectException>>(
                new TagProjectNotFoundException(projectId, tagId)));
    }

    private async Task<Result<TagsProject, TagProjectException>> DeleteEntity(TagsProject tagsProject,
        CancellationToken cancellationToken)
    {
        try
        {
            return await repository.Delete(tagsProject, cancellationToken);
        }
        catch (Exception e)
        {
            return new TagProjectUnknownException(tagsProject.ProjectId, tagsProject.TagId, e);
        }
    }
}