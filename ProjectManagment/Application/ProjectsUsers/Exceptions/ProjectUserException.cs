using Domain.Projects;
using Domain.Users;

namespace Application.ProjectsUsers.Exceptions;

public class ProjectUserException(
    ProjectId? projectId,
    UserId? userId,
    string message,
    Exception? innerException = null)
    : Exception(message, innerException)
{
    public ProjectId? ProjectId { get; } = projectId;
    public UserId? UserId { get; } = userId;
}

public class ProjectNotFound(ProjectId projectId, UserId userId)
    : ProjectUserException(projectId, null, $"Project {projectId} not found, while tried to add user {userId}");

public class UserNotFound(ProjectId projectId, UserId userId)
    : ProjectUserException(projectId, userId, $"User {userId} not found, while tried to add to project {projectId}");

public class UserAlreadyInProject(ProjectId projectId, UserId userId)
    : ProjectUserException(projectId, userId, $"User {userId} already in project");

public class ProjectUserNotFound(ProjectId projectId, UserId userId)
    : ProjectUserException(projectId, userId, $"User {userId} in project {projectId} not found");

public class ProjectUserUnknownException(ProjectId projectId, UserId userId, Exception innerException)
    : ProjectUserException(projectId, userId, $"Unknown exception in project {projectId} with user {userId}",
        innerException);