using FluentValidation;

namespace Application.ProjectTasks.Commands;

public class CreateTaskForProjectCommandValidator : AbstractValidator<CreateTaskForProjectCommand>
{
    public CreateTaskForProjectCommandValidator()
    {
        RuleFor(x=>x.TaskTitle).NotEmpty().MaximumLength(50).MinimumLength(4);
        RuleFor(x=>x.ShortDescription).NotEmpty().MaximumLength(100).MinimumLength(10);
        RuleFor(x=>x.ProjectId).NotEmpty();
        RuleFor(x=>x.CategoryId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}