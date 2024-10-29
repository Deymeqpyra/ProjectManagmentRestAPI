using Domain.Categories;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<Option<Category>> GetById(CategoryId categoryId, CancellationToken cancellationToken);
    Task<Option<Category>> GetByName(string Name, CancellationToken cancellationToken);
    Task<Category> Create(Category category, CancellationToken cancellationToken);
    Task<Category> Update(Category category, CancellationToken cancellationToken);
    Task<Category> Delete(Category category, CancellationToken cancellationToken);
}