using Domain.Tasks;

namespace Api.Dtos.TasksDto;

public record UserTaskDto(
    Guid? TaskId,
    string TaskName)
{
    public static UserTaskDto FromTask(ProjectTask task)
        => new(
            TaskId: task.TaskId.value,
            TaskName: task.Title);
}