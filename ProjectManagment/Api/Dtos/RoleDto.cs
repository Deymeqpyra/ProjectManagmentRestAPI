using Domain.Roles;

namespace Api.Dtos;

public record RoleDto(
    Guid? roleId,
    string RoleName)
{
    public static RoleDto FromDomainModel(Role role)
        => new(
            roleId: role.Id.value,
            RoleName: role.Name);
}