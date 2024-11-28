using Application.ProjectsUsers.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ProjectUserErrorHandler
{
    public static ObjectResult ToObjectResult(this ProjectUserException projectUserException)
    {
        return new ObjectResult(projectUserException.Message)
        {
            StatusCode = projectUserException switch
            {
                ProjectNotFound => StatusCodes.Status404NotFound,
                UserNotFound => StatusCodes.Status404NotFound,
                UserNotEnoughPremission => StatusCodes.Status403Forbidden,
                UserAlreadyInProject => StatusCodes.Status409Conflict,
                ProjectUserNotFound => StatusCodes.Status404NotFound,
                ProjectUserUnknownException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}