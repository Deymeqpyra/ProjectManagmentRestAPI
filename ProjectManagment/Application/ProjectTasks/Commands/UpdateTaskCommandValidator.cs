using FluentValidation;

namespace Application.ProjectTasks.Commands;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x=>x.TitleUpdate).NotEmpty().MaximumLength(50).MinimumLength(4);
        RuleFor(x=>x.DescriptionUpdate).NotEmpty().MaximumLength(100).MinimumLength(10);
        RuleFor(x=>x.CategoryIdUpdate).NotEmpty();
    }
}