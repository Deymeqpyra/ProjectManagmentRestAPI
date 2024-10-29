using FluentValidation;

namespace Application.Categories.Commands;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x=>x.CategoryName).MaximumLength(50).MinimumLength(3).NotEmpty();
    }
}