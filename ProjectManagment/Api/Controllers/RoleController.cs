using Api.Dtos.RoleDto;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Roles.Commands;
using Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("role")]
[ApiController]
public class RoleController(ISender sender, IRoleQueries roleQueries ) : ControllerBase
{
    [HttpGet("GetAll")]
    public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetAllRoles(CancellationToken cancellationToken)
    {
        var entities = await roleQueries.GetAll(cancellationToken);
        
        return entities.Select(RoleDto.FromDomainModel).ToList();
    }

    [HttpGet("GetById/{roleId:guid}")]
    public async Task<ActionResult<RoleDto>> GetRoleById([FromRoute] Guid roleId, CancellationToken cancellationToken)
    {
        var entity = await roleQueries.GetById(new RoleId(roleId), cancellationToken);
        
        return entity.Match<ActionResult<RoleDto>>(
            r=>RoleDto.FromDomainModel(r),
            () => NotFound());
    }

    [HttpPost("AddNewRole")]
    public async Task<ActionResult<RoleDto>> Create([FromBody] string name, CancellationToken cancellationToken)
    {
        var input = new CreateRoleCommand
        {
            Name = name
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }

    [HttpPut("UpdateRole/{roleId:guid}")]
    public async Task<ActionResult<RoleDto>> Update(
        [FromRoute] Guid roleId,
        [FromBody] string updateName,
        CancellationToken cancellationToken)
    {
        var input = new UpdateRoleCommand
        {
            roleId = roleId,
            updateName = updateName
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }

    [HttpDelete("DeleteRole/{roleId:guid}")]
    public async Task<ActionResult<RoleDto>> Delete([FromRoute] Guid roleId, CancellationToken cancellationToken)
    {
        var input = new DeleteRoleCommand
        {
            RoleID = roleId
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }
}