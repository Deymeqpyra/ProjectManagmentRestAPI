using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository, ICategoryQueries
{
    public async Task<Option<Category>> GetById(CategoryId categoryId, CancellationToken cancellationToken)
    {
        var category = await context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.Id == categoryId, cancellationToken);
        
        return category == null ? Option.None<Category>() : Option.Some(category);
    }

    public async Task<Option<Category>> GetByName(string Name, CancellationToken cancellationToken)
    {
        var entity = await context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == Name);
        
        return entity == null ? Option.None<Category>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<Category>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Category> Create(Category category, CancellationToken cancellationToken)
    {
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync(cancellationToken);
        
        return category;
    }

    public async Task<Category> Update(Category category, CancellationToken cancellationToken)
    {
        context.Categories.Update(category);
        await context.SaveChangesAsync(cancellationToken);
        
        return category;
    }

    public async Task<Category> Delete(Category category, CancellationToken cancellationToken)
    {
        context.Categories.Remove(category);
        await context.SaveChangesAsync(cancellationToken);
        
        return category;
    }
}