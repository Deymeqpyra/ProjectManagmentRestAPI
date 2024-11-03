using FluentValidation;

namespace Application.Projects.Commands;

public class SetStatusProjectCommandValidator : AbstractValidator<SetStatusProjectCommand>
{
    public SetStatusProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.ProjectStatusId).NotEmpty();
    }
}