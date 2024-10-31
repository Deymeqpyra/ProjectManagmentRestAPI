using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Roles.Exceptions;
using Domain.Roles;
using MediatR;

namespace Application.Roles.Commands;

public class CreateRoleCommand : IRequest<Result<Role, RoleException>>
{
    public required string Name { get; init; }
}

public class CreateRoleCommandHandler(IRoleRepository repository)
    : IRequestHandler<CreateRoleCommand, Result<Role, RoleException>>
{
    public async Task<Result<Role, RoleException>> Handle(CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var existingRole = await repository.GetByName(request.Name, cancellationToken);

        return await existingRole.Match(
            r => Task.FromResult<Result<Role, RoleException>>(new RoleAlreadyExistsException(r.Id)),
            async () => await CreateEntity(request.Name, cancellationToken));
    }

    private async Task<Result<Role, RoleException>> CreateEntity(
        string roleName,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Role.New(RoleId.New(), roleName);
            
            return await repository.Create(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new RoleUnknownException(RoleId.Empty, e);
        }
    }
}