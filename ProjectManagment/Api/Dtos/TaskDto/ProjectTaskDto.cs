using Domain.Tasks;

namespace Api.Dtos.TaskDto;

public record ProjectTaskDto(
    Guid? TaskId,
    string Title, 
    string Description,
    Guid CategoryId)
{
    public static ProjectTaskDto FromProjectTask(ProjectTask projectTask)
    => new(
        TaskId: projectTask.ProjectTaskId.value,
        Title: projectTask.Title,
        Description: projectTask.ShortDescription,
        CategoryId: projectTask.CategoryId.Value);
}