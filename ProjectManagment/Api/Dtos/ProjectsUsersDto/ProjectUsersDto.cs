using Domain.Projects;
using Domain.ProjectUsers;

namespace Api.Dtos.ProjectsUsersDto;

public record ProjectUsersDto(Guid? projectId, Guid? userId)
{
    public static ProjectUsersDto FromProject(ProjectUser project)
        => new(projectId: project.ProjectId.value, userId: project.UserId.value);
}