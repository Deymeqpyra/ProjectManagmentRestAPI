namespace Domain.ProjectPriorities;

public class ProjectPriorityId(Guid value)
{
    public ProjectPriorityId New() => new(Guid.NewGuid());
    public ProjectPriorityId Empty() => new(Guid.Empty);

    public override string ToString() => value.ToString();
}