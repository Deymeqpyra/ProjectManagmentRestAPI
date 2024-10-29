using FluentValidation;

namespace Application.Categories.Commands;

public class CreateCategoryCommandValidatior : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidatior()
    {
        RuleFor(x => x.Name).MaximumLength(50).MinimumLength(3).NotEmpty();
    }
}