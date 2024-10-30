using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Priorities.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("priorities")]
[ApiController]
public class PriorityController(ISender sender, IPriorityQueries priorityQueries) : ControllerBase
{
    [HttpGet("GetAllPriorities")]
    public async Task<ActionResult<IReadOnlyList<PriorityDto>>> GetAllPriority(CancellationToken cancellationToken)
    {
        var entities = await priorityQueries.GetAll(cancellationToken);
        
        return entities.Select(PriorityDto.FromDomainModel).ToList();
    }
    [HttpPost("CreatePriority")]
    public async Task<ActionResult<PriorityDto>> Create([FromBody] string Name, CancellationToken cancellationToken)
    {
        var input = new CreatePriorityCommand
        {
            Name = Name
        };
        
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<PriorityDto>>(
            p=>PriorityDto.FromDomainModel(p),
            e=>e.ToObjectResult());
    }
    
}