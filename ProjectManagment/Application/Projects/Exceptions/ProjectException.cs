using Application.Statuses.Exceptions;
using Domain.Priorities;
using Domain.Projects;
using Domain.Statuses;

namespace Application.Projects.Exceptions;

public class ProjectException(ProjectId projectId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public ProjectId ProjectId { get; } = projectId;
}

public class ProjectNotFound(ProjectId projectId)
    : ProjectException(projectId, $"Project {projectId} not found");

public class ProjectAlreadyExist(ProjectId projectId)
    : ProjectException(projectId, $"Project {projectId} already exists");

public class ProjectUnknownException(ProjectId projectId, Exception innerException)
    : ProjectException(projectId, $"Project {projectId} unknown exception", innerException);

public class StatusNotFound(ProjectId projectId, ProjectStatusId statusId)
    : ProjectException(projectId, $"Status was not found {statusId}, when tried to set to project {projectId}");

public class PriorityNotFound(ProjectId projectId, ProjectPriorityId priorityId)
    : ProjectException(projectId, $"Priority was not found {priorityId}, when tried to set to project {projectId}");