using Domain.Projects;

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