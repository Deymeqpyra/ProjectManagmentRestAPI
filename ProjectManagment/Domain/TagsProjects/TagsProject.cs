using Domain.Projects;
using Domain.Tags;

namespace Domain.TagsProjects;

public class TagsProject
{
    public Guid Id { get; } = Guid.NewGuid();

    public ProjectId ProjectId { get; private set; }
    public Project? Project { get; }

    public TagId TagId { get; private set; }
    public Tag? Tag { get; }

    private TagsProject(ProjectId projectId, TagId tagId)
    {
        ProjectId = projectId;
        TagId = tagId;
    }

    public static TagsProject New(ProjectId projectId, TagId tagId)
        => new(projectId, tagId);
}