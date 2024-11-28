using Application.Comments.Exceptions;
using Domain.Comments;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class CommentErrorHandler
{
    public static ObjectResult ToObjectResult(this CommentException comment)
    {
        return new ObjectResult(comment.Message)
        {
            StatusCode = comment switch
            {
                CommentNotFound => StatusCodes.Status404NotFound,
                CommentUnknownException => StatusCodes.Status500InternalServerError,
                ProjectNotFound => StatusCodes.Status404NotFound,
                UserNotFound => StatusCodes.Status404NotFound,
                UserNotEnoughPremission => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}