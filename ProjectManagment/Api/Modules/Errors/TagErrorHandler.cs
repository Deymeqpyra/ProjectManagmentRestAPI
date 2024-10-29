using Application.Tags.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class TagErrorHandler
{
    public static ObjectResult ToObjectResult(this Exception exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                TagNotFoundException => StatusCodes.Status404NotFound,
                TagAlreadyExistsException => StatusCodes.Status409Conflict,
                TagUnknownException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}