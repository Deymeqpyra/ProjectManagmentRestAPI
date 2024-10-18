namespace Domain.ProjectPriorities;

public record ProjectPriorityId(Guid value)
{
    public ProjectPriorityId New() => new(Guid.NewGuid());
    public ProjectPriorityId Empty() => new(Guid.Empty);

    public override string ToString() => value.ToString();
}