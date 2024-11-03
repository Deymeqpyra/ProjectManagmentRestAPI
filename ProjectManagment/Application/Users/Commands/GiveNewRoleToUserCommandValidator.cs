using FluentValidation;

namespace Application.Users.Commands;

public class GiveNewRoleToUserCommandValidator : AbstractValidator<GiveNewRoleToUserCommand>
{
    public GiveNewRoleToUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleId).NotEmpty();
    }
}