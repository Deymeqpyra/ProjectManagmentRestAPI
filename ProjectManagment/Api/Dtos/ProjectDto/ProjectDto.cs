using Api.Dtos.PrioritiesDto;
using Api.Dtos.ProjectsUsersDto;
using Api.Dtos.StatusesDto;
using Api.Dtos.TagsProjects;
using Api.Dtos.UsersDto;
using Domain.Priorities;
using Domain.Projects;

namespace Api.Dtos.ProjectDto;

public record ProjectDto(
    Guid? projectId,
    string Title,
    string Description,
    Guid StatusId,
    StatusDto? Status,
    Guid PriorityId,
    PriorityDto? Priority,
    List<TagForProjectDto> tagProjects,
    List<UserForProjectDto> userProjects,
    List<string> Comments)
{
    public static ProjectDto FromProject(Project project)
    => new(projectId: project.ProjectId.value,
        Title: project.Title,
        Description: project.Description,
        StatusId: project.ProjectStatusId.value,
        Status: project.ProjectStatus == null ? null : StatusDto.FromDomainModel(project.ProjectStatus),
        PriorityId: project.ProjectPriorityId.value,
        Comments: project.Comments,
        Priority: project.ProjectPriority == null ? null : PriorityDto.FromDomainModel(project.ProjectPriority),
        tagProjects: project.TagsProjects.Select(TagForProjectDto.FromTag).ToList(),
        userProjects: project.ProjectUsers.Select(UserForProjectDto.FromUserShortInfo).ToList()
        );
}