using Application.Categories.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Categories;
using MediatR;

namespace Application.Categories.Commands;

public class UpdateCategoryCommand : IRequest<Result<Category, CategoryException>>
{
    public required Guid CategoryId { get; init; }
    public required string CategoryName { get; init; }
}

public class UpdateCategoryCommandHandler(ICategoryRepository repository)
    : IRequestHandler<UpdateCategoryCommand, Result<Category, CategoryException>>
{
    public async Task<Result<Category, CategoryException>> Handle(
        UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categoryId = new CategoryId(request.CategoryId);
        var exisitingCategory = await repository.GetById(
            categoryId,
            cancellationToken);

        return await exisitingCategory.Match(
            async c => await UpdateEntity(c, request.CategoryName, cancellationToken),
            () => Task.FromResult<Result<Category, CategoryException>>(new CategoryNotFoundException(categoryId)));
    }

    private async Task<Result<Category, CategoryException>> UpdateEntity(
        Category category,
        string name,
        CancellationToken cancellationToken)
    {
        try
        {
            category.UpdateDetails(name);

            return await repository.Update(category, cancellationToken);
        }
        catch (Exception e)
        {
            return new CategoryUnknownException(CategoryId.Empty(), e);
        }
    }
}