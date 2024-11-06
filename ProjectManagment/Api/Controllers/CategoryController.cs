using Api.Dtos.CategoriesDto;
using Api.Modules.Errors;
using Application.Categories.Commands;
using Application.Common.Interfaces.Queries;
using Domain.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("categories")]
[ApiController]
[Authorize]
public class CategoryController(ISender sender, ICategoryQueries categoryQueries) : ControllerBase
{
    [Authorize(Roles = "Admin, User")]
    [HttpGet("GetCategories")]
    public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCategories(CancellationToken cancellationToken)
    {
        var entities = await categoryQueries.GetAll(cancellationToken);

        return entities.Select(CategoryDto.FromDomainModel).ToList();
    }

    [Authorize(Roles = "Admin, User")]
    [HttpGet("GetCategory/{categoryId:guid}")]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        var entity = await categoryQueries.GetById(new CategoryId(categoryId), cancellationToken);

        return entity.Match<ActionResult<CategoryDto>>(
            c => CategoryDto.FromDomainModel(c),
            () => NotFound());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("CreateCategory")]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] string categoryName,
        CancellationToken cancellationToken)
    {
        var input = new CreateCategoryCommand
            { Name = categoryName };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CategoryDto>>(
            c => CategoryDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("UpdateCategory/{categoryId:guid}")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(
        [FromRoute] Guid categoryId,
        [FromBody] string categoryName,
        CancellationToken cancellationToken)
    {
        var catId = new CategoryId(categoryId);
        var input = new UpdateCategoryCommand
        {
            CategoryId = catId.Value,
            CategoryName = categoryName
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CategoryDto>>(
            c => CategoryDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteCategory/{categoryId:guid}")]
    public async Task<ActionResult<CategoryDto>> Delete([FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        var catId = new CategoryId(categoryId);
        var input = new DeleteCategoryCommand
        {
            CategoryId = catId.Value
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CategoryDto>>(
            c => CategoryDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }
}