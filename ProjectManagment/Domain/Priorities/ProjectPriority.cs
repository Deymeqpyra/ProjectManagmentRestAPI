namespace Domain.Priorities;

public class ProjectPriority
{
    public ProjectPriorityId Id { get; }
    
    public string Name { get; private set; }

    private ProjectPriority(ProjectPriorityId id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public static ProjectPriority New(ProjectPriorityId id, string name)
    => new(id, name);

    public void UpdateDetaile(string priorityName)
    {
        Name = priorityName;
    }
}