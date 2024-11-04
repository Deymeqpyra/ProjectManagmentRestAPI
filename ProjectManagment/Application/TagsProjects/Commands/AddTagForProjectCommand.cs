using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.TagsProjects.Exceptions;
using Domain.Projects;
using Domain.Tags;
using Domain.TagsProjects;
using MediatR;

namespace Application.TagsProjects.Commands;

public class AddTagForProjectCommand : IRequest<Result<TagsProject, TagProjectException>>
{
    public required Guid ProjectId { get; init; }
    public required Guid TagId { get; init; }
}

public class AddTagForProjectHandler(
    ITagProjectRepository tagProjectRepository,
    IProjectRepository projectRepository,
    ITagRepository tagRepository)
    : IRequestHandler<AddTagForProjectCommand, Result<TagsProject, TagProjectException>>
{
    public async Task<Result<TagsProject, TagProjectException>> Handle(AddTagForProjectCommand request,
        CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var tagId = new TagId(request.TagId);
        var exsitingTagProject = await tagProjectRepository.GetByTagAndProjectId(tagId, projectId, cancellationToken);

        return await exsitingTagProject.Match(
            tp => Task.FromResult<Result<TagsProject, TagProjectException>>(
                new TagProjectAlreadyExsist(projectId, tagId)),
            async () =>
            {
                var exisitingProject = await projectRepository.GetById(projectId, cancellationToken);
                return await exisitingProject.Match(
                    async p =>
                    {
                        var exisitngTag = await tagRepository.GetById(tagId, cancellationToken);
                        return await exisitngTag.Match(
                            async t => await AddTag(p.ProjectId, t.Id, cancellationToken),
                            () => Task.FromResult<Result<TagsProject, TagProjectException>>(
                                new TagNotFoundException(tagId)));
                    },
                    () => Task.FromResult<Result<TagsProject, TagProjectException>>(
                        new ProjectNotFoundException(projectId)));
            }
        );
    }

    private async Task<Result<TagsProject, TagProjectException>> AddTag(
        ProjectId projectId,
        TagId tagId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = TagsProject.New(projectId, tagId);

            return await tagProjectRepository.Create(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new TagProjectUnknownException(projectId, tagId, e);
        }
    }
}