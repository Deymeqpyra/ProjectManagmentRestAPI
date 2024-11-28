using Api.Dtos.TasksDto;
using FluentValidation;

namespace Api.Modules.Validators.Tasks;

public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {
        RuleFor(x=>x.title).NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(50)
            .WithMessage("Title is too long.(max: 50)")
            .MinimumLength(3)
            .WithMessage("Title is too short.(min: 3)");
    }
}