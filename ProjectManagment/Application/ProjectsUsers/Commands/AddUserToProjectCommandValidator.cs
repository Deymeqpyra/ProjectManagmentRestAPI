using FluentValidation;

namespace Application.ProjectsUsers.Commands;

public class AddUserToProjectCommandValidator : AbstractValidator<AddUserToProjectCommand>
{
    public AddUserToProjectCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}