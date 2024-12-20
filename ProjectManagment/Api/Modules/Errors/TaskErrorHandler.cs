using Application.ProjectTasks.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class TaskErrorHandler
{
    public static ObjectResult ToObjectResult(this TaskException taskException)
    {
        return new ObjectResult(taskException.Message)
        {
            StatusCode = taskException switch
            {
                TaskNotFoundException _ => StatusCodes.Status404NotFound,
                TaskAlreadyFinished => StatusCodes.Status409Conflict,
                TaskAlreadyExistsException => StatusCodes.Status409Conflict,
                TaskUnknowException => StatusCodes.Status500InternalServerError,
                UserNotFoundWhileCreated => StatusCodes.Status404NotFound,
                NotEnoughPermission => StatusCodes.Status403Forbidden,
                ProjectAlreadyFinished => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}