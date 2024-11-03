using FluentValidation;

namespace Application.Users.Commands;

public class UpdateUserNameCommandValidator : AbstractValidator<UpdateUserNameCommand>
{
    public UpdateUserNameCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x=>x.UserName).NotEmpty().MinimumLength(3).MaximumLength(30);
    }
}