using FluentValidation;

namespace Application.Roles.Commands;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x=>x.roleId).NotEmpty();
        RuleFor(x=>x.updateName).NotEmpty().MaximumLength(50).MinimumLength(3);
    }
}