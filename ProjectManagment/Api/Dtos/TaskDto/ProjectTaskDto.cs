using Domain.Tasks;

namespace Api.Dtos.TaskDto;

public record ProjectTaskDto(
    Guid? TaskId,
    string Title, 
    bool? completed,
    string Description,
    Guid CategoryId)
{
    public static ProjectTaskDto FromProjectTask(ProjectTask projectTask)
    => new(
        TaskId: projectTask.TaskId.value,
        Title: projectTask.Title,
        completed: projectTask.IsFinished,
        Description: projectTask.ShortDescription,
        CategoryId: projectTask.CategoryId.Value);
}