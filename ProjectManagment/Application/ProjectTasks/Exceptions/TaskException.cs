using Domain.Categories;
using Domain.Projects;
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

public class ProjectAlreadyFinished(ProjectTaskId taskId, ProjectId projectId)
    : TaskException(taskId, $"Project with if {projectId} already finished");   
public class UserNotFoundWhileCreated(ProjectTaskId taskId)
    : TaskException(taskId, $"User not found");
public class UserNotEnoughPremission(ProjectTaskId taskId)
    : TaskException(taskId, $"User not enough permissions");
public class CategoryNotFound(ProjectTaskId taskId, CategoryId categoryId)
    : TaskException(taskId, $"Category with id: {categoryId} not found");
    
public class NotEnoughPermission(ProjectTaskId taskId)
    : TaskException(taskId, $"You don't have enough permission to perform this action ");