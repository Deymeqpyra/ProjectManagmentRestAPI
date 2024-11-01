using Domain.Categories;

namespace Api.Dtos.CategoryDto;

public record CategoryDto(
    Guid? CategoryId,
    string Name)
{
    public static CategoryDto FromDomainModel(Category category)
        => new(
            CategoryId: category.Id.Value,
            Name: category.Name);
}