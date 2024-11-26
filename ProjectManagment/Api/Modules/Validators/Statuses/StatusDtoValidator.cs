using Api.Dtos.StatusesDto;
using FluentValidation;

namespace Api.Modules.Validators.Statuses;

public class StatusDtoValidator : AbstractValidator<CreateStatusDto>
{
    public StatusDtoValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3).WithMessage("Name is too short (min:3)")
            .MaximumLength(30).WithMessage("Name is too long (max: 30)")
            .NotEmpty().WithMessage("Name is required");
    }
}