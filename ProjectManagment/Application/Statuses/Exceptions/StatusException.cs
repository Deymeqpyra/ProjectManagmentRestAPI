using Domain.Statuses;

namespace Application.Statuses.Exceptions;

public class StatusException(ProjectStatusId statusId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public ProjectStatusId statusId { get; } = statusId;
}

public class StatusNotFoundException(ProjectStatusId statusId)
    : StatusException(statusId, $"No status found with id {statusId}");

public class StatusAlreadyExistsException(ProjectStatusId statusId)
    : StatusException(statusId, $"A status with this id {statusId} is already exists");

public class StatusUnknownException(ProjectStatusId statusId, Exception innerException)
    : StatusException(statusId, $"A status with this id {statusId} is not a known status", innerException);