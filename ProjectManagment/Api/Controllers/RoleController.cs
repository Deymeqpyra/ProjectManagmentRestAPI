using Api.Dtos.RolesDto;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Roles.Commands;
using Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("role")]
[ApiController]
[Authorize]
public class RoleController(ISender sender, IRoleQueries roleQueries) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet("GetAll")]
    public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetAllRoles(CancellationToken cancellationToken)
    {
        var entities = await roleQueries.GetAll(cancellationToken);

        return entities.Select(RoleDto.FromDomainModel).ToList();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetById/{roleId:guid}")]
    public async Task<ActionResult<RoleDto>> GetRoleById([FromRoute] Guid roleId, CancellationToken cancellationToken)
    {
        var entity = await roleQueries.GetById(new RoleId(roleId), cancellationToken);

        return entity.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            () => NotFound());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("AddNewRole")]
    public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleDto createRoleDto, CancellationToken cancellationToken)
    {
        var input = new CreateRoleCommand
        {
            Name = createRoleDto.Name
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("UpdateRole/{roleId:guid}")]
    public async Task<ActionResult<RoleDto>> Update(
        [FromRoute] Guid roleId,
        [FromBody] CreateRoleDto createRoleDto,
        CancellationToken cancellationToken)
    {
        var input = new UpdateRoleCommand
        {
            roleId = roleId,
            updateName = createRoleDto.Name
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
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