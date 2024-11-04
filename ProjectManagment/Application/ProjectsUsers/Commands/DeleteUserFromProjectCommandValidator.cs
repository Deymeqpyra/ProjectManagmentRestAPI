using FluentValidation;

namespace Application.ProjectsUsers.Commands;

public class DeleteUserFromProjectCommandValidator : AbstractValidator<DeleteUserFromProjectCommand>
{
    public DeleteUserFromProjectCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x=>x.ProjectId).NotEmpty();
    }
}