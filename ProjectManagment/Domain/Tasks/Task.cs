using Domain.Categories;
using Domain.Users;

namespace Domain.Tasks;

public class Task
{
    public TaskId TaskId { get; }
    
    public string Title { get; private set; }
    public string ShortDescription { get; private set; }
    
    public bool IsFinished { get; private set; }
    
    public CategoryId CategoryId { get;  }
    public Category? Category { get;  }

    public ICollection<User> Users { get; } = [];
    
}