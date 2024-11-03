using Domain.Tasks;

namespace Api.Dtos.TaskDto;

public record CreateTaskDto(
    string title,
    string description,
    Guid projectId,
    Guid categoryId)
{
    public static CreateTaskDto FromTask(ProjectTask task)
    => new(
        title: task.Title,
        description: task.ShortDescription,
        projectId: task.ProjectId.value,    
        categoryId: task.CategoryId.Value);
}