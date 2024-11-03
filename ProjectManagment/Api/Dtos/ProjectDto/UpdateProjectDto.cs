using Domain.Projects;

namespace Api.Dtos.ProjectDto;

public record UpdateProjectDto(string Title, string Description)
{
    public static UpdateProjectDto FromProject(Project project)
        => new(project.Title, project.Description);
}