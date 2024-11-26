using Domain.Roles;

namespace Application.Roles.Exceptions;

public class RoleException(RoleId roleId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public RoleId RoleId { get; } = roleId;
}

public class RoleNotFoundException(RoleId roleId)
    : RoleException(roleId, $"Role with id: {roleId} not found");

public class RoleAlreadyExistsException(RoleId roleId)
    : RoleException(roleId, $"Role with id: {roleId} already exists");

public class RoleUnknownException(RoleId roleId, Exception innerException)
    : RoleException(roleId, $"Role with id: {roleId} expect an unknown exception", innerException);