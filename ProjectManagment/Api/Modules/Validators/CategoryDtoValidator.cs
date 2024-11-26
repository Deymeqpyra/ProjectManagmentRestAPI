using Api.Dtos.CategoriesDto;
using FluentValidation;

namespace Api.Modules.Validators;

public class CategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name should be at least 3 characters.")
            .MaximumLength(50).WithMessage("Name is too long");
    }
}