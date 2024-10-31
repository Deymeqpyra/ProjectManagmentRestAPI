using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Roles.Exceptions;
using Domain.Roles;
using MediatR;

namespace Application.Roles.Commands;

public class DeleteRoleCommand : IRequest<Result<Role, RoleException>>
{
    public required Guid RoleID { get; init; }
}

public class DeleteRoleCommandHandler(IRoleRepository repository)
    : IRequestHandler<DeleteRoleCommand, Result<Role, RoleException>>
{
    public async Task<Result<Role, RoleException>> Handle(DeleteRoleCommand request,
        CancellationToken cancellationToken)
    {
        var exisitingRole = await repository.GetById(new RoleId(request.RoleID), cancellationToken);

        return await exisitingRole.Match(
            async r => await DeleteEntity(r, cancellationToken),
            () => Task.FromResult<Result<Role, RoleException>>(
                new RoleNotFoundException(new RoleId(request.RoleID)))
        );
    }

    private async Task<Result<Role, RoleException>> DeleteEntity(Role role, CancellationToken cancellationToken)
    {
        try
        {
            return await repository.Delete(role, cancellationToken);
        }
        catch (Exception e)
        {
            return new RoleUnknownException(role.Id, e);
        }
    }
}