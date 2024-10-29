using Application.Categories.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class CatergoryErrorHandler
{
    public static ObjectResult ToObjectResult(this CategoryException categoryException)
    {
        return new ObjectResult(categoryException.Message)
        {
            StatusCode = categoryException switch
            {
                CategoryNotFoundException => StatusCodes.Status404NotFound,
                CategoryAlreadyExistsException => StatusCodes.Status409Conflict,
                CategoryUnknownException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}