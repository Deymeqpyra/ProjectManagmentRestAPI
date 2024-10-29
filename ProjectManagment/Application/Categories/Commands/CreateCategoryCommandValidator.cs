using FluentValidation;

namespace Application.Categories.Commands;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).MaximumLength(50).MinimumLength(3).NotEmpty();
    }
}