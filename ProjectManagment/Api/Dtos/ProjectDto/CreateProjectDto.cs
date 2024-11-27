using Api.Dtos.CommentsDto;
using Domain.Projects;

namespace Api.Dtos.ProjectDto;

public record CreateProjectDto(
    string Title,
    string Description,
    Guid statusId, 
    Guid priorityId)
{
    public static CreateProjectDto FromProject(Project project)
        => new(
            project.Title,
            project.Description,
            project.ProjectStatusId.value,
            project.ProjectPriorityId.value);
}