using Domain.Categories;
using Domain.Projects;
using Domain.Users;

namespace Domain.Tasks;

public class ProjectTask
{
    public ProjectTaskId ProjectTaskId { get; }
    
    public string Title { get; private set; }
    public string ShortDescription { get; private set; }
    
    public bool IsFinished { get; private set; }
    
    public ProjectId ProjectId { get; private set; }
    public Project? Project { get;  }
    
    public CategoryId CategoryId { get;  }
    public Category? Category { get;  }
}