using Domain.TagsProjects;

namespace Application.Common.Interfaces.Repositories;

public interface ITagProjectRepository
{
    Task<TagsProject> Create(TagsProject tagProject, CancellationToken cancellationToken);
    Task<TagsProject> Update(TagsProject tagProject, CancellationToken cancellationToken);
    Task<TagsProject> Delete(TagsProject tagProject, CancellationToken cancellationToken);
}