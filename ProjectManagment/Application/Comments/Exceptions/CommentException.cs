using Domain.Comments;
using Domain.Projects;
using Domain.Users;

namespace Application.Comments.Exceptions;

public class CommentException(CommentId commentId, string message, Exception? innerException = null)
    : Exception(message, innerException)

{
    public CommentId commentId { get; } = commentId;
}

public class CommentNotFound(CommentId commentId)
    : CommentException(commentId, $"Comment {commentId} not found");

public class CommentUnknownException(CommentId commentId, Exception innerException)
    : CommentException(commentId, $"Comment unknown exception {commentId}", innerException);

public class ProjectNotFound(ProjectId projectId, CommentId commentId)
    : CommentException(commentId, $"Project with id {projectId} not found, while creating a new comment {commentId}");

public class UserNotFound(UserId userId, CommentId commentId)
    : CommentException(commentId, $"User with id {userId} not found, while creating a new comment {commentId}");