using Application.Projects.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ProjectErrorHandler
{
    public static ObjectResult ToObjectResult(this ProjectException projectException)
    {
        return new ObjectResult(projectException.Message)
        {
            StatusCode = projectException switch
            {
                ProjectNotFound => StatusCodes.Status404NotFound,
                ProjectAlreadyExist => StatusCodes.Status409Conflict,
                ProjectUnknownException => StatusCodes.Status400BadRequest,
                UserNotFoundWhileCreated => StatusCodes.Status404NotFound,
                NotEnoughPermission => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}