using FluentValidation;

namespace Application.Users.Commands;

public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(x=>x.UserId).NotEmpty();
        RuleFor(x=>x.Password).NotEmpty().MinimumLength(6).MaximumLength(50);
    }
}