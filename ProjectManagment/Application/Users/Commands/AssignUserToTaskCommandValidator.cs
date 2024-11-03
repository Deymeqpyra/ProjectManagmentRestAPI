using FluentValidation;

namespace Application.Users.Commands;

public class AssignUserToTaskCommandValidator : AbstractValidator<AssignUserToTaskCommand>
{
    public AssignUserToTaskCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x=>x.TaskId).NotEmpty();
    }
}