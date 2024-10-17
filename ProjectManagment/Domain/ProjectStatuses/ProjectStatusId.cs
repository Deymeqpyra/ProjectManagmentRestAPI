namespace Domain.ProjectStatuses;

public class ProjectStatusId(Guid value)
{
    public static ProjectStatusId New() => new (Guid.NewGuid());
    public static ProjectStatusId Empty() => new (Guid.Empty);

    public override string ToString() => value.ToString();
}