using FluentValidation;

namespace Application.Statuses.Commands;

public class UpdateStatusCommandValidator : AbstractValidator<UpdateStatusCommand>
{
    public UpdateStatusCommandValidator()
    {
        RuleFor(x=>x.StatusName).NotEmpty().MaximumLength(50).MinimumLength(3);
        RuleFor(x => x.StatusId).NotEmpty();
    }
}