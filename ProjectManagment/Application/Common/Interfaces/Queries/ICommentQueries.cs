using Domain.Comments;

namespace Application.Common.Interfaces.Queries;

public interface ICommentQueries
{
    Task<IReadOnlyList<Comment>> GetAll(CancellationToken cancellationToken);
}