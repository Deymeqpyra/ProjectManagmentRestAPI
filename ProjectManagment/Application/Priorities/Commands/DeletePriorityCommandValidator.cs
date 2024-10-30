using FluentValidation;

namespace Application.Priorities.Commands;

public class DeletePriorityCommandValidator : AbstractValidator<DeletePriorityCommand>
{
    public DeletePriorityCommandValidator()
    {
        RuleFor(x=>x.PriorityId).NotEmpty();
    }
}