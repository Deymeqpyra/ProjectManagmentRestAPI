using Api.Dtos.TagsDto;
using FluentValidation;

namespace Api.Modules.Validators.Tags;

public class TagDtoValidators : AbstractValidator<CreateTagDto>
{
    public TagDtoValidators()
    {
        RuleFor(x=>x.Name).NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name is too short. (min: 3)")
            .MaximumLength(50).WithMessage("Name is too long. (min: 50)");
    }
}