using Api.Dtos.CategoriesDto;
using Api.Dtos.UsersDto;
using Domain.Tasks;

namespace Api.Dtos.TasksDto;

public record ProjectTaskDto(
    Guid? TaskId,
    string Title, 
    bool? completed,
    string Description,
    Guid CategoryId,
    CategoryDto? Category,
    UserTaskShortInfo userTask)
{   
    public static ProjectTaskDto FromProjectTask(ProjectTask projectTask)
    => new(
        TaskId: projectTask.TaskId.value,
        Title: projectTask.Title,
        completed: projectTask.IsFinished,
        Description: projectTask.ShortDescription,
        CategoryId: projectTask.CategoryId.Value,
        Category:  projectTask.Category == null ? null : CategoryDto.FromDomainModel(projectTask.Category),
        userTask: projectTask.User == null ? null : UserTaskShortInfo.FromUser(projectTask.User)
        );
}