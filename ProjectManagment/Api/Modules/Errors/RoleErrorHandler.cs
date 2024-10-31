using Application.Priorities.Exceptions;
using Application.Roles.Exceptions;
using Domain.Roles;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class RoleErrorHandler
{
    public static ObjectResult ToObjectResult(this RoleException roleException)
    {
        return new ObjectResult(roleException.Message)
        {
            StatusCode = roleException switch
            {
                RoleNotFoundException => StatusCodes.Status404NotFound,
                RoleAlreadyExistsException => StatusCodes.Status409Conflict,
                RoleUnknownException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}