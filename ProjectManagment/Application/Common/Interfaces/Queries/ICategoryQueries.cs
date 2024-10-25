using Domain.Categories;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ICategoryQueries
{
    Task<Option<Category>> GetById(CategoryId categoryId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Category>> GetAll(CancellationToken cancellationToken);
}