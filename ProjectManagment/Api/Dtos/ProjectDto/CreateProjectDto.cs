using Api.Dtos.CommentsDto;
using Domain.Projects;

namespace Api.Dtos.ProjectDto;

public record CreateProjectDto(
    string Title,
    string Description,
    Guid priorityId)
{
    public static CreateProjectDto FromProject(Project project)
        => new(
            project.Title,
            project.Description,
            project.ProjectPriorityId.value);
}