using Domain.Categories;

namespace Api.Dtos.CategoriesDto;

public record CreateCategoryDto(string Name)
{
    public static CreateCategoryDto FromCategory(Category category)
    => new (Name: category.Name);
}