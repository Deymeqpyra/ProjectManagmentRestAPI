using Application.Priorities.Exceptions;
using Application.Users.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class UserErrorHandler
{
    public static ObjectResult ToObjectResult(this UserException userException)
    {
        return new ObjectResult(userException.Message)
        {
            StatusCode = userException switch
            {
                UserNotFoundException => StatusCodes.Status404NotFound,
                UserAlreadyAssinedToTask => StatusCodes.Status409Conflict,
                UserAlreadyExistsException => StatusCodes.Status409Conflict,
                UserUnknownException => StatusCodes.Status500InternalServerError,
                RoleNotFound => StatusCodes.Status404NotFound,
                TaskNotFound => StatusCodes.Status404NotFound,
                WrongCrenditals => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}