using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Comments;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class CommentRepository(ApplicationDbContext context) : ICommentRepository, ICommentQueries
{
    public async Task<IReadOnlyList<Comment>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Comments
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public async Task<Option<Comment>> GetById(CommentId id, CancellationToken cancellationToken)
    {
        var comment = await context.Comments
            .Include(x => x.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return comment == null ? Option.None<Comment>() : Option.Some(comment);
    }

    public async Task<IReadOnlyList<Comment>> GetByUserId(UserId userId, CancellationToken cancellationToken)
    {
        return await context.Comments
            .Include(x => x.User)
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Comment> Create(Comment comment, CancellationToken cancellationToken)
    {
        await context.Comments.AddAsync(comment, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return comment;
    }

    public async Task<Comment> Update(Comment comment, CancellationToken cancellationToken)
    {
        context.Comments.Update(comment);
        await context.SaveChangesAsync(cancellationToken);
        
        return comment;
    }

    public async Task<Comment> Delete(Comment comment, CancellationToken cancellationToken)
    {
        context.Comments.Remove(comment);
        await context.SaveChangesAsync(cancellationToken);
        
        return comment;
    }
}