using FluentValidation;

namespace Application.Priorities.Commands;

public class UpdatePriorityCommandValidator : AbstractValidator<UpdatePriorityCommand>
{
    public UpdatePriorityCommandValidator()
    {
        RuleFor(x=>x.PriorityId).NotEmpty();
        RuleFor(x=>x.UpdateName).NotEmpty().MaximumLength(50).MinimumLength(3);
    }
}