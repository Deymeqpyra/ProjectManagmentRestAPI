using FluentValidation;

namespace Application.Projects.Commands;

public class UpdateProjectDetailsCommandValidator : AbstractValidator<UpdateProjectDetailsCommand>
{
    public UpdateProjectDetailsCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x=>x.UpdateTitle).NotEmpty().MaximumLength(50).MinimumLength(3);
        RuleFor(x=>x.UpdateDescription).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}