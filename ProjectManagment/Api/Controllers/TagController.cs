using Api.Dtos.TagsDto;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Tags.Commands;
using Domain.Tags;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("tags")]
[ApiController]
[Authorize]
public class TagController(ISender sender, ITagQueries tagQueries) : ControllerBase
{
    [Authorize(Roles = "Admin, User")]
    [HttpGet("GetAllTags")]
    public async Task<ActionResult<IReadOnlyList<TagDto>>> GetAllTags(CancellationToken cancellationToken)
    {
        var entities = await tagQueries.GetAll(cancellationToken);

        return entities.Select(TagDto.FromDomainModel).ToList();
    }

    [Authorize(Roles = "Admin, User")]
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

    [Authorize(Roles = "Admin")]
    [HttpPost("AddTag")]
    public async Task<ActionResult<TagDto>> AddTag(
        [FromBody] CreateTagDto tagDto,
        CancellationToken cancellationToken)
    {
        var input = new CreateTagCommand
        {
            Name = tagDto.Name
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<TagDto>>(
            t => TagDto.FromDomainModel(t),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("UpdateTag/{tagId:guid}")]
    public async Task<ActionResult<TagDto>> UpdateTag(
        [FromRoute] Guid tagId,
        [FromBody] CreateTagDto tagDto,
        CancellationToken cancellationToken)
    {
        var input = new UpdateTagCommand
        {
            Id = tagId,
            Name = tagDto.Name
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<TagDto>>(
            t => TagDto.FromDomainModel(t),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
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
            t => TagDto.FromDomainModel(t),
            e => e.ToObjectResult());
    }
}