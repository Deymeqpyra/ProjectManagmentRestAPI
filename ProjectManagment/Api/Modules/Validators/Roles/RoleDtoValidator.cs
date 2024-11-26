using Api.Dtos.RolesDto;
using FluentValidation;

namespace Api.Modules.Validators.Roles;

public class RoleDtoValidator : AbstractValidator<CreateRoleDto>
{
    public RoleDtoValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3).WithMessage("Role name is too short (min:3)")
            .MaximumLength(30).WithMessage("Name is too long (max: 30)")
            .NotEmpty().WithMessage("Name is required");
    }
}