using Domain.Priorities;
using Domain.ProjectUsers;
using Domain.Statuses;
using Domain.Tags;
using Domain.TagsProjects;

namespace Domain.Projects;

public class Project
{
    public ProjectId ProjectId { get; }
    
    public string Title { get; private set; }
    public string Description { get; private set; }
    
    public DateTime LastUpdate { get; private set; }
    
    public ProjectStatusId ProjectStatusId { get; }
    public ProjectStatus? ProjectStatus { get;  }
    
    public ProjectPriorityId ProjectPriorityId { get;  }
    public ProjectPriority? ProjectPriority { get;  }

    public ICollection<TagsProject> TagsProjects { get; } = [];
    public ICollection<ProjectUser> ProjectUsers { get; } = [];
    public List<string> Comments { get; private set; } 
}