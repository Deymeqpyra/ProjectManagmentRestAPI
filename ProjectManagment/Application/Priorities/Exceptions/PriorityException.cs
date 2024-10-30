using Domain.Priorities;

namespace Application.Priorities.Exceptions;

public class PriorityException(ProjectPriorityId priorityId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public ProjectPriorityId PriorityId { get; } = priorityId;
}

public class PriorityNotFoundException(ProjectPriorityId priorityId)
    : PriorityException(priorityId, $"Priority with id: {priorityId} not found");

public class PriorityAlreadyExistsException(ProjectPriorityId priorityId)
    : PriorityException(priorityId, $"Priority with id: {priorityId} already exists");
public class PriorityUnknownException(ProjectPriorityId priorityId, Exception innerException)
: PriorityException(priorityId, $"Unknown exception for priority with id: {priorityId} ", innerException); 