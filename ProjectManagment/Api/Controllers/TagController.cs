using Api.Dtos.TagDto;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Tags.Commands;
using Domain.Tags;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers;

[Route("tags")]
[ApiController]
public class TagController(ISender sender, ITagQueries tagQueries) : ControllerBase
{
    [HttpGet("GetAllTags")]
    public async Task<ActionResult<IReadOnlyList<TagDto>>> GetAllTags(CancellationToken cancellationToken)
    {
        var entities = await tagQueries.GetAll(cancellationToken);
        
        return entities.Select(TagDto.FromDomainModel).ToList();
    }
    [HttpGet("GetTagById/{tagId:guid}")]
    public async Task<ActionResult<TagDto>> GetAllTags(
        [FromRoute] Guid tagId, 
        CancellationToken cancellationToken)
    {
        var entity = await tagQueries.GetById(new TagId(tagId), cancellationToken);

        return entity.Match<ActionResult<TagDto>>(
            t => TagDto.FromDomainModel(t),
            () => NotFound());
    }

    [HttpPost("AddTag")]
    public async Task<ActionResult<TagDto>> AddTag(
        [FromBody] string name,
        CancellationToken cancellationToken)
    {
        var input = new CreateTagCommand
        {
            Name = name
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<TagDto>>(
            t => TagDto.FromDomainModel(t),
            e => e.ToObjectResult());
    }

    [HttpPut("UpdateTag/{tagId:guid}")]
    public async Task<ActionResult<TagDto>> UpdateTag(
        [FromRoute] Guid tagId,
        [FromBody] string name,
        CancellationToken cancellationToken)
    {
        var input = new UpdateTagCommand
        {
            Id = tagId,
            Name = name
        };
        
        var result = await sender.Send(input, cancellationToken); 
        
        return result.Match<ActionResult<TagDto>>(
            t=>TagDto.FromDomainModel(t),
            e=>e.ToObjectResult());
    }

    [HttpDelete("DeleteTag/{tagId:guid}")]
    public async Task<ActionResult<TagDto>> DeleteTag(
        [FromRoute] Guid tagId, 
        CancellationToken cancellationToken)
    {
        var input = new DeleteTagCommand
        {
            TagId = tagId
        };
        
        var result = await sender.Send(input, cancellationToken);
        
        return result.Match<ActionResult<TagDto>>(
            t=>TagDto.FromDomainModel(t),
            e=>e.ToObjectResult());
    }
}