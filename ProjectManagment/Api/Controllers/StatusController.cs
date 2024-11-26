using Api.Dtos.StatusesDto;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Statuses.Commands;
using Domain.Statuses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("status")]
[ApiController]
[Authorize]
public class StatusController(ISender sender, IStatusQueries statusQueries) : ControllerBase
{
    [Authorize(Roles = "Admin, User")]
    [HttpGet("GetAll")]
    public async Task<ActionResult<IReadOnlyList<StatusDto>>> GetAllStatus(CancellationToken cancellationToken)
    {
        var entites = await statusQueries.GetAll(cancellationToken);

        return entites.Select(StatusDto.FromDomainModel).ToList();
    }

    [Authorize(Roles = "Admin, User")]
    [HttpGet("GetById/{statusId:guid}")]
    public async Task<ActionResult<StatusDto>> GetById([FromRoute] Guid statusId, CancellationToken cancellationToken)
    {
        var entity = await statusQueries.GetById(new ProjectStatusId(statusId), cancellationToken);

        return entity.Match<ActionResult<StatusDto>>(
            s => StatusDto.FromDomainModel(s),
            () => NotFound());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Create")]
    public async Task<ActionResult<StatusDto>> Create([FromBody] CreateStatusDto statusDto, CancellationToken cancellationToken)
    {
        var input = new CreateStatusCommand
        {
            Name = statusDto.Name
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<StatusDto>>(
            s => StatusDto.FromDomainModel(s),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("Update/{statusId:guid}")]
    public async Task<ActionResult<StatusDto>> Update(
        [FromRoute] Guid statusId,
        [FromBody] CreateStatusDto statusDto,
        CancellationToken cancellationToken)
    {
        var input = new UpdateStatusCommand
        {
            StatusId = statusId,
            StatusName = statusDto.Name
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<StatusDto>>(
            s => StatusDto.FromDomainModel(s),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("Delete/{statusId:guid}")]
    public async Task<ActionResult<StatusDto>> Delete(
        [FromRoute] Guid statusId,
        CancellationToken cancellationToken)
    {
        var input = new DeleteStatusCommand
        {
            StatusId = statusId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<StatusDto>>(
            s => StatusDto.FromDomainModel(s),
            e => e.ToObjectResult());
    }
}