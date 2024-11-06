using FluentValidation;

namespace Application.ProjectTasks.Commands;

public class FinishTaskCommandValidator : AbstractValidator<FinishTaskCommand>
{
    public FinishTaskCommandValidator()
    {
        RuleFor(x=>x.TaskId).NotEmpty();
        RuleFor(x=>x.UserId).NotEmpty();
    }
}