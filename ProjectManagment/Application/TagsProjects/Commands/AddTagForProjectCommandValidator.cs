using FluentValidation;

namespace Application.TagsProjects.Commands;

public class AddTagForProjectCommandValidator : AbstractValidator<AddTagForProjectCommand>
{
    public AddTagForProjectCommandValidator()
    {
        RuleFor(x=>x.TagId).NotEmpty();
        RuleFor(x=>x.ProjectId).NotEmpty();
    }
}