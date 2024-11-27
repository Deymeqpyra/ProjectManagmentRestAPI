using Application.Projects.Exceptions;
using Domain.Projects;
using Domain.Tags;
using Domain.Users;

namespace Application.TagsProjects.Exceptions;

public class TagProjectException(ProjectId? projectId, TagId? tagId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public ProjectId ProjectId { get; } = projectId;
    public TagId TagId { get; } = tagId;
}

public class ProjectNotFoundException(ProjectId projectId)
    : TagProjectException(projectId, null, $"Project {projectId} not found");

public class TagProjectAlreadyExsist(ProjectId projectId, TagId tagId)
    : TagProjectException(projectId, tagId, $"Project {projectId} already have tag {tagId}");

public class UserNotEnoughPremission(ProjectId projectId, UserId userId)
    : TagProjectException(projectId, null, $"User {userId} not enough permissions for {projectId}");

public class UserNotFound(ProjectId projectId, UserId userId)
    : TagProjectException(projectId, null, $"User with id: {userId} does not exist");
public class TagNotFoundException(TagId tagId)
    : TagProjectException(null, tagId, $"Tag {tagId} not found");

public class TagProjectNotFoundException(ProjectId projectId, TagId tagId)
    : TagProjectException(projectId, tagId, $"ProjectTag with tag {tagId} and project {projectId} not found");

public class TagProjectUnknownException(ProjectId projectId, TagId tagId, Exception innerException)
    : TagProjectException(projectId, tagId, $"Unknown exception for tag {tagId} or project {projectId}",
        innerException);