using FluentValidation;

namespace Application.Tags.Commands;

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(25).MinimumLength(3); 
    }
}