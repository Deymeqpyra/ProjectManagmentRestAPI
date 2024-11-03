using Domain.Categories;

namespace Api.Dtos.CategoriesDto;

public record CategoryDto(
    Guid? CategoryId,
    string Name)
{
    public static CategoryDto FromDomainModel(Category category)
        => new(
            CategoryId: category.Id.Value,
            Name: category.Name);
}