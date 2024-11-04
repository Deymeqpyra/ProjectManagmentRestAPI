using Domain.Projects;
using Domain.TagsProjects;

namespace Api.Dtos.TagsProjects;

public record TagProjectDto(
    Guid ProjectId, 
    Guid TagId)
{
    public static TagProjectDto FromProject(TagsProject tagsProject)
        => new(
            ProjectId: tagsProject.ProjectId.value, 
            TagId: tagsProject.TagId.value);
}