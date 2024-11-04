using FluentValidation;

namespace Application.TagsProjects.Commands;

public class DeleteTagFromProjectCommandValidator : AbstractValidator<DeleteTagFromProjectCommand>
{
    public DeleteTagFromProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.TagId).NotEmpty();
    }
}