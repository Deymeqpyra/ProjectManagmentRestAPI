using Domain.Comments;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ICommentRepository
{
    Task<Option<Comment>> GetById(CommentId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Comment>> GetByUserId(UserId userId, CancellationToken cancellationToken);
    Task<Comment> Create(Comment comment, CancellationToken cancellationToken);
    Task<Comment> Update(Comment comment, CancellationToken cancellationToken);
    Task<Comment> Delete(Comment comment, CancellationToken cancellationToken);
}