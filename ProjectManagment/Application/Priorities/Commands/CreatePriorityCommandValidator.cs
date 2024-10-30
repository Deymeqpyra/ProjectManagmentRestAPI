using FluentValidation;

namespace Application.Priorities.Commands;

public class CreatePriorityCommandValidator : AbstractValidator<CreatePriorityCommand>
{
    public CreatePriorityCommandValidator()
    {
        RuleFor(x=>x.Name).MaximumLength(50).MinimumLength(3).NotEmpty();
    }
}