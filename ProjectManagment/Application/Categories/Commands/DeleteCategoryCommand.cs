using Application.Categories.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Categories;
using MediatR;

namespace Application.Categories.Commands;

public class DeleteCategoryCommand : IRequest<Result<Category, CategoryException>>
{
    public required Guid CategoryId { get; init; }
}

public class DeleteCategoryCommandHandler(ICategoryRepository repository)
    : IRequestHandler<DeleteCategoryCommand, Result<Category, CategoryException>>
{
    public async Task<Result<Category, CategoryException>> Handle(
        DeleteCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        var categoryId = new CategoryId(request.CategoryId);
        
        var exisitingCategory = await repository.GetById(categoryId, cancellationToken);
        
        return await exisitingCategory.Match<Task<Result<Category, CategoryException>>>(
            async c => await DeleteEntity(c, cancellationToken),
            () => Task.FromResult<Result<Category, CategoryException>>(new CategoryNotFoundException(categoryId)));
    }

    public async Task<Result<Category, CategoryException>> DeleteEntity(
        Category category,
        CancellationToken cancellationToken)
    {
        try
        {
            return await repository.Delete(category, cancellationToken);
        }
        catch (Exception e)
        {
            return new CategoryUnknownException(CategoryId.Empty(), e);
        }
    }
    
}