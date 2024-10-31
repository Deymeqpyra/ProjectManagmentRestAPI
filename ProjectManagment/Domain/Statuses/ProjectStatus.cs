namespace Domain.Statuses;

public class ProjectStatus
{
    public ProjectStatusId Id { get; }
    public string Name { get; private set; }

    private ProjectStatus(ProjectStatusId id, string name)
    {
        Id = id;
        Name = name;
    }

    public static ProjectStatus New(ProjectStatusId id, string name)
        => new(id, name);

    public void UpdateDetails(string name)
    {
        Name = name;
    }
}