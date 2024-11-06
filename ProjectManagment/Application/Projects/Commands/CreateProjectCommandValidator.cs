using FluentValidation;

namespace Application.Projects.Commands;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x=>x.PriorityId).NotEmpty();
        RuleFor(x=>x.StatusId).NotEmpty();
        RuleFor(x=>x.UserId).NotEmpty();
        RuleFor(x=>x.Title).NotEmpty().MaximumLength(25).MinimumLength(3);
        RuleFor(x=>x.Description).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}