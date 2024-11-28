using Domain.Tasks;

namespace Api.Dtos.TasksDto;

public record CreateTaskDto(
    string title,
    string description,
    Guid categoryId)
{
    public static CreateTaskDto FromTask(ProjectTask task)
    => new(
        title: task.Title,
        description: task.ShortDescription,
        categoryId: task.CategoryId.Value);
}