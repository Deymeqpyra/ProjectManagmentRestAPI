using Domain.Projects;
using Domain.Tags;
using Domain.TagsProjects;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ITagProjectRepository
{
    Task<Option<TagsProject>> GetByTagAndProjectId(TagId tagId, ProjectId projectId,
        CancellationToken cancellationToken);
    Task<TagsProject> Create(TagsProject tagProject, CancellationToken cancellationToken);
    Task<TagsProject> Update(TagsProject tagProject, CancellationToken cancellationToken);
    Task<TagsProject> Delete(TagsProject tagProject, CancellationToken cancellationToken);
}