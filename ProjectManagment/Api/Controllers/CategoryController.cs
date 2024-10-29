using Api.Dtos;
using Api.Modules.Errors;
using Application.Categories.Commands;
using Application.Common.Interfaces.Queries;
using Domain.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("categories")]
[ApiController]
public class CategoryController(ISender sender, ICategoryQueries categoryQueries) : ControllerBase
{
    [HttpGet("GetCategories")]
    public async  Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCategories(CancellationToken cancellationToken)
    {
        var entities = await categoryQueries.GetAll(cancellationToken);
        
        return entities.Select(CategoryDto.FromDomainModel).ToList();
    }

    [HttpPost("CreateCategory")]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateCategoryCommand
            { Name = request.Name };
        
        var result  = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CategoryDto>>(
            c => CategoryDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }
}