using FluentValidation;

namespace Application.Projects.Commands;

public class ChangeProjectPriorityCommandValidator : AbstractValidator<ChangeProjectPriorityCommand>
{
    public ChangeProjectPriorityCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x=>x.PriorityId).NotEmpty();
        RuleFor(x=>x.UserId).NotEmpty();
    }
}