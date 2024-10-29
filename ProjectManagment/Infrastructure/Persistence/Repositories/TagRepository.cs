using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Tags;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class TagRepository(ApplicationDbContext context) : ITagRepository, ITagQueries
{
    public async Task<Option<Tag>> GetById(TagId tagId, CancellationToken cancellationToken)
    {
        var entity = await context.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == tagId);
        
        return entity == null ? Option.None<Tag>() : Option.Some(entity); 
    }

    public async Task<Option<Tag>> GetByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name);
        
        return entity == null ? Option.None<Tag>() : Option.Some(entity);
    }
    public async Task<IReadOnlyList<Tag>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Tags
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Tag> Create(Tag tag, CancellationToken cancellationToken)
    {
        await context.Tags.AddAsync(tag, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return tag;
    }

    public async Task<Tag> Update(Tag tag, CancellationToken cancellationToken)
    {
        context.Tags.Update(tag);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return tag;
    }

    public async Task<Tag> Delete(Tag tag, CancellationToken cancellationToken)
    {
        context.Tags.Remove(tag);
        
        await context.SaveChangesAsync(cancellationToken);

        return tag;
    }
}