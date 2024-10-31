using Application.Statuses.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class StatusErrorHandler
{
    public static ObjectResult ToObjectResult(this StatusException statusException)
    {
        return new ObjectResult(statusException.Message)
        {
            StatusCode = statusException switch
            {
                StatusNotFoundException => StatusCodes.Status404NotFound,
                StatusAlreadyExistsException => StatusCodes.Status409Conflict,
                StatusUnknownException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}