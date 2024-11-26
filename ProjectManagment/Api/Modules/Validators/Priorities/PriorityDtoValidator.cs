using Api.Dtos.PrioritiesDto;
using FluentValidation;

namespace Api.Modules.Validators.Priorities;

public class PriorityDtoValidator : AbstractValidator<CreatePriorityDto>
{
    public PriorityDtoValidator()
    {
        RuleFor(x=>x.title)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name at least 3 characters.")
            .MaximumLength(50).WithMessage("Name is too long.");
    }
}