using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Roles.Exceptions;
using Domain.Roles;
using MediatR;

namespace Application.Roles.Commands;

public class UpdateRoleCommand : IRequest<Result<Role, RoleException>>
{
    public required Guid roleId { get; init; }
    public required string updateName { get; init; }
}

public class UpdateRoleCommandHandler(IRoleRepository repository)
    : IRequestHandler<UpdateRoleCommand, Result<Role, RoleException>>
{
    public async Task<Result<Role, RoleException>> Handle(UpdateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var exisitingRole = await repository.GetById(new RoleId(request.roleId), cancellationToken);

        return await exisitingRole.Match(
            async r => await UpdateEntity(r, request.updateName, cancellationToken),
            () => Task.FromResult<Result<Role, RoleException>>
                (new RoleNotFoundException(new RoleId(request.roleId))));
    }

    private async Task<Result<Role, RoleException>> UpdateEntity(
        Role role,
        string name,
        CancellationToken cancellationToken)
    {
        try
        {
            role.UpdateDetails(name);

            return await repository.Update(role, cancellationToken);
        }
        catch (Exception e)
        {
            return new RoleUnknownException(RoleId.Empty(), e);
        }
    }
}