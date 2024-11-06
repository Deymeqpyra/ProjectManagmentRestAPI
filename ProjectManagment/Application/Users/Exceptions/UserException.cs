using Domain.Roles;
using Domain.Tasks;
using Domain.Users;

namespace Application.Users.Exceptions;

public class UserException(UserId userId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public UserId UserId { get; } = userId;
}

public class UserNotFoundException(UserId userId)
    : UserException(userId, $"User {userId} not found");

public class UserAlreadyExistsException(UserId userId)
    : UserException(userId, $"User {userId} already exists");

public class UserUnknownException(UserId userId, Exception innerException)
    : UserException(userId, $"User {userId} unknown, {innerException}", innerException);

public class RoleNotFound(UserId userId, RoleId roleId)
    : UserException(userId, $"Role {roleId} not found, while trying to assign to user {userId}");

public class UserAlreadyAssinedToTask(UserId userId, ProjectTaskId taskId)
    : UserException(userId, $"User {userId} already assigned to task {taskId}");

public class TaskNotFound(UserId userId, ProjectTaskId taskId)
    : UserException(userId, $"Task {taskId} not found, while trying to assign to user {userId}");
public class WrongCrenditals(UserId userId)
    : UserException(userId, $"Wrong password or email address");