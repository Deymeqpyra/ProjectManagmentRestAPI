using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.TagsProjects.Exceptions;
using Domain.Projects;
using Domain.Tags;
using Domain.TagsProjects;
using Domain.Users;
using MediatR;

namespace Application.TagsProjects.Commands;

public class DeleteTagFromProjectCommand : IRequest<Result<TagsProject, TagProjectException>>
{
    public required Guid ProjectId { get; init; }
    public required Guid UserId { get; init; }
    public required Guid TagId { get; init; }
}

public class DeleteTagFromProjectCommandHandler(
    ITagProjectRepository repository,
    IUserRepository userRepository,
    IProjectRepository projectRepository)
    : IRequestHandler<DeleteTagFromProjectCommand, Result<TagsProject, TagProjectException>>
{
    public async Task<Result<TagsProject, TagProjectException>> Handle(DeleteTagFromProjectCommand request,
        CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var tagId = new TagId(request.TagId);
        var userId = new UserId(request.UserId);

        var existingTagProject = await repository.GetByTagAndProjectId(tagId, projectId, cancellationToken);
        var existingUser = await userRepository.GetById(userId, cancellationToken);
        var exstingProject = await projectRepository.GetById(projectId, cancellationToken);
        return await existingUser.Match(
            async u =>
            {
                return await exstingProject.Match(
                    async p =>
                    {
                        if (p.UserId == userId || u.Role!.Name == "Admin")
                        {
                            return await existingTagProject.Match(
                                async tg => await DeleteEntity(tg, cancellationToken),
                                () => Task.FromResult<Result<TagsProject, TagProjectException>>(
                                    new TagProjectNotFoundException(projectId, tagId)));
                        }
                        return await Task.FromResult<Result<TagsProject, TagProjectException>>(
                            new UserNotEnoughPremission(projectId, userId));
                       
                    },
                    () => Task.FromResult<Result<TagsProject, TagProjectException>>(
                        new ProjectNotFoundException(projectId))
                );
            },
            () => Task.FromResult<Result<TagsProject, TagProjectException>>(new UserNotFound(projectId, userId)));
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