using Domain.Tasks;

namespace Application.ProjectTasks.Exceptions;

public class TaskException(ProjectTaskId taskId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public ProjectTaskId TaskId { get; } = taskId;
}

public class TaskNotFoundException(ProjectTaskId taskId)
    : TaskException(taskId, $"Task with id {taskId} not found");

public class TaskAlreadyExistsException(ProjectTaskId taskId)
    : TaskException(taskId, $"Task with id {taskId} already exists");

public class TaskAlreadyFinished(ProjectTaskId taskId)
    : TaskException(taskId, $"Task with id {taskId} already finished");

public class TaskUnknowException(ProjectTaskId taskId, Exception innerException)
    : TaskException(taskId, $"Unknown exception for task with id: {taskId} ", innerException);