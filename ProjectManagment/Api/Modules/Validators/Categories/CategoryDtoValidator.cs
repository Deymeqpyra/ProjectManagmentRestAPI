using Api.Dtos.CategoriesDto;
using FluentValidation;

namespace Api.Modules.Validators.Categories;

public class CategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name is too short. (min: 3)")
            .MaximumLength(50).WithMessage("Name is too long. (max: 50)");
    }
}