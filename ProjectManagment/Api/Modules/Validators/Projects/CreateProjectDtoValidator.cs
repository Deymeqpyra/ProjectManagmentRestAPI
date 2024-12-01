using Api.Dtos.ProjectDto;
using FluentValidation;

namespace Api.Modules.Validators.Projects;

public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x=>x.Title)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name is too short.(max: 3)")
            .MaximumLength(50).WithMessage("Name is too long.(max: 50)");
        RuleFor(x=>x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MinimumLength(3).WithMessage("Description is too short.(max: 3)")
            .MaximumLength(255).WithMessage("Description is too long.(max: 255)");
        RuleFor(x => x.priorityId)
            .NotEmpty().WithMessage("PriorityId is required.");
    }
}