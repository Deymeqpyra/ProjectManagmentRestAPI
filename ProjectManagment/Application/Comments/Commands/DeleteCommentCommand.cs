using Application.Comments.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Comments;
using Domain.Users;
using MediatR;

namespace Application.Comments.Commands;

public class DeleteCommentCommand : IRequest<Result<Comment, CommentException>>
{
    public required Guid CommentId { get; set; }
    public required Guid UserId { get; set; }
}

public class DeleteCommentCommandHandler(IUserRepository userRepository, ICommentRepository commentRepository)
    : IRequestHandler<DeleteCommentCommand, Result<Comment, CommentException>>
{
    public async Task<Result<Comment, CommentException>> Handle(DeleteCommentCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var commentId = new CommentId(request.CommentId);

        var exisitingUser = await userRepository.GetById(userId, cancellationToken);
        var exisitingComment = await commentRepository.GetById(commentId, cancellationToken);

        return await exisitingUser.Match(
            async user =>
            {
                return await exisitingComment.Match(
                    async comment => await DeleteEntity(comment, user, cancellationToken),
                    () => Task.FromResult<Result<Comment, CommentException>>(new CommentNotFound(commentId)));
            },
            () => Task.FromResult<Result<Comment, CommentException>>(new UserNotFound(userId, commentId)));
    }

    private async Task<Result<Comment, CommentException>> DeleteEntity(Comment comment, User user,
        CancellationToken cancellationToken)
    {
        try
        {
            if (comment.UserId.value == user.Id.value || user.Role!.Name == "Admin")
            {
                return await commentRepository.Delete(comment, cancellationToken);
            }

            return await Task.FromResult<Result<Comment, CommentException>>(
                new UserNotEnoughPremission(user.Id, comment.Id));
        }
        catch (Exception e)
        {
            return new CommentUnknownException(comment.Id, e);
        }
    }
}