using Domain.Projects;
using Domain.Tags;
using Domain.TagsProjects;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ITagProjectQueries
{
    Task<IReadOnlyList<TagsProject>> GetByProjectId(ProjectId projectId, CancellationToken cancellationToken);
    Task<IReadOnlyList<TagsProject>> GetByTagId(TagId tagId, CancellationToken cancellationToken);

    Task<Option<TagsProject>> GetByTagAndProjectId(TagId tagId, ProjectId projectId,
        CancellationToken cancellationToken);
    Task<IReadOnlyList<TagsProject>> GetByAll(CancellationToken cancellationToken);
}