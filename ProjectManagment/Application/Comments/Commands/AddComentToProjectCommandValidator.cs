using FluentValidation;

namespace Application.Comments.Commands;

public class AddComentToProjectCommandValidator : AbstractValidator<AddComentToProjectCommand>
{
    public AddComentToProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x=>x.CommentMessage).NotEmpty().MinimumLength(3).MaximumLength(255);
    }
}