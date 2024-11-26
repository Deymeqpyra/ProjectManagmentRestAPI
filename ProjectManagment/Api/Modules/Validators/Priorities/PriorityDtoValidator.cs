using Api.Dtos.PrioritiesDto;
using FluentValidation;

namespace Api.Modules.Validators.Priorities;

public class PriorityDtoValidator : AbstractValidator<CreatePriorityDto>
{
    public PriorityDtoValidator()
    {
        RuleFor(x=>x.title)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name is too short. (min: 3)")
            .MaximumLength(50).WithMessage("Name is too long. (max: 50)");
    }
}