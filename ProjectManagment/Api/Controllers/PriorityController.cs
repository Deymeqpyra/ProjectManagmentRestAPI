using Api.Dtos.PrioritiesDto;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Priorities.Commands;
using Domain.Priorities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("priorities")]
[ApiController]
[Authorize]
public class PriorityController(ISender sender, IPriorityQueries priorityQueries) : ControllerBase
{
    [Authorize(Roles = "Admin, User")]
    [HttpGet("GetAllPriorities")]
    public async Task<ActionResult<IReadOnlyList<PriorityDto>>> GetAllPriority(CancellationToken cancellationToken)
    {
        var entities = await priorityQueries.GetAll(cancellationToken);

        return entities.Select(PriorityDto.FromDomainModel).ToList();
    }

    [Authorize(Roles = "Admin, User")]
    [HttpGet("GetById/{priorityId:guid}")]
    public async Task<ActionResult<PriorityDto>> GetById([FromRoute] Guid priorityId,
        CancellationToken cancellationToken)
    {
        var entity = await priorityQueries.GetById(new ProjectPriorityId(priorityId), cancellationToken);

        return entity.Match<ActionResult<PriorityDto>>(
            p => PriorityDto.FromDomainModel(p),
            () => NotFound()
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("CreatePriority")]
    public async Task<ActionResult<PriorityDto>> Create([FromBody] CreatePriorityDto priorityDto, CancellationToken cancellationToken)
    {
        var input = new CreatePriorityCommand
        {
            Name = priorityDto.title
        };

        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<PriorityDto>>(
            p => PriorityDto.FromDomainModel(p),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("UpdatePriority/{priorityId:guid}")]
    public async Task<ActionResult<PriorityDto>> Update(
        [FromRoute] Guid priorityId,
        [FromBody] CreatePriorityDto priorityDto,
        CancellationToken cancellationToken)
    {
        var input = new UpdatePriorityCommand
        {
            PriorityId = priorityId,
            UpdateName = priorityDto.title
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<PriorityDto>>
        (
            p => PriorityDto.FromDomainModel(p),
            e => e.ToObjectResult()
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("DeletePriority/{priorityId:guid}")]
    public async Task<ActionResult<PriorityDto>> Delete([FromRoute] Guid priorityId,
        CancellationToken cancellationToken)
    {
        var input = new DeletePriorityCommand
        {
            PriorityId = priorityId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<PriorityDto>>(
            p => PriorityDto.FromDomainModel(p),
            e => e.ToObjectResult()
        );
    }
}