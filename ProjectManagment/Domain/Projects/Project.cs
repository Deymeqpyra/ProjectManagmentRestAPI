using Domain.Priorities;
using Domain.ProjectUsers;
using Domain.Statuses;
using Domain.TagsProjects;
using Domain.Users;

namespace Domain.Projects;

public class Project
{
    public ProjectId ProjectId { get; }

    public string Title { get; private set; }
    public string Description { get; private set; }

    public DateTime LastUpdate { get; private set; }
    public ProjectStatusId ProjectStatusId { get; private set;  }
    public ProjectStatus? ProjectStatus { get; }

    public ProjectPriorityId ProjectPriorityId { get; private set; }
    public ProjectPriority? ProjectPriority { get; }
    public ICollection<TagsProject> TagsProjects { get; } = [];
    public ICollection<ProjectUser> ProjectUsers { get; } = [];
    
    public UserId UserId { get; private set; }
    public User? User { get;  }
    public List<string> Comments { get; private set; } = [];

    private Project(
        ProjectId projectId,
        string title,
        string description,
        DateTime lastUpdate,
        UserId userId,
        ProjectStatusId projectStatusId,
        ProjectPriorityId projectPriorityId
        )
    {
        ProjectId = projectId;
        Title = title;
        Description = description;
        LastUpdate = lastUpdate;
        UserId = userId;
        ProjectStatusId = projectStatusId;
        ProjectPriorityId = projectPriorityId;
    }

    public static Project New(
        ProjectId projectId,
        string title,
        string description,
        UserId userId,
        ProjectStatusId projectStatusId,
        ProjectPriorityId projectPriorityId)
        => new(projectId, title, description, DateTime.UtcNow, userId, projectStatusId, projectPriorityId);

    public void UpdateDetails(
        string updateTitle, 
        string updateDescription 
        )
    {
        Title = updateTitle;
        Description = updateDescription;
        LastUpdate = DateTime.UtcNow;
    }

    public void AddComment(string comment)
    {
        Comments.Add(comment);
        LastUpdate = DateTime.UtcNow;
    }

    public void SetStatus(ProjectStatusId projectStatusId)
    {
        ProjectStatusId = projectStatusId;
        LastUpdate = DateTime.UtcNow;
    }

    public void ChangePriority(ProjectPriorityId projectPriorityId)
    {
        ProjectPriorityId = projectPriorityId;
        LastUpdate = DateTime.UtcNow;
    }
}