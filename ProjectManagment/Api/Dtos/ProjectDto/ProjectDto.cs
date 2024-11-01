using Domain.Priorities;
using Domain.Projects;
using Domain.Statuses;

namespace Api.Dtos.ProjectDto;

public record ProjectDto(
    Guid? projectId,
    string Title,
    string Description,
    Guid StatusId,
    Guid PriorityId,
    List<string> Comments)
{
    public static ProjectDto FromProject(Project project)
    => new(projectId: project.ProjectId.value,
        Title: project.Title,
        Description: project.Description,
        StatusId: project.ProjectStatusId.value,
        PriorityId: project.ProjectPriorityId.value,
        Comments: project.Comments
        );
}