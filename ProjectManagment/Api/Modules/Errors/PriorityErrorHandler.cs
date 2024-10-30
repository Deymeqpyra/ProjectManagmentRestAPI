using Application.Categories.Exceptions;
using Application.Priorities.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class PriorityErrorHandler
{
    public static ObjectResult ToObjectResult(this PriorityException priorityException)
    {
        return new ObjectResult(priorityException.Message)
        {
            StatusCode = priorityException switch
            {
                PriorityNotFoundException => StatusCodes.Status404NotFound,
                PriorityAlreadyExistsException => StatusCodes.Status409Conflict,
                PriorityUnknownException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}