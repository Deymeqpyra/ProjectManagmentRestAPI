using Domain.Projects;
using Domain.Users;

namespace Domain.ProjectUsers;

public class ProjectUser
{
    public Guid ProjectUserId { get; } = Guid.NewGuid();
    public ProjectId ProjectId { get; private set; }
    public Project? Project { get; }

    public UserId UserId { get; private set; }
    public User? User { get; }

    private ProjectUser(ProjectId projectId, UserId userId)
    {
        ProjectId = projectId;
        UserId = userId;
    }

    public static ProjectUser New(ProjectId projectId, UserId userId)
        => new(projectId, userId);
}