using Domain.Categories;
using FluentValidation;

namespace Application.Categories.Commands;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(command => command.CategoryId).NotEmpty();
    }
}