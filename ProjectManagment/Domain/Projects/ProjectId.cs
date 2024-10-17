namespace Domain.Projects;

public class ProjectId(Guid value)
{
    public static ProjectId New() => new(Guid.NewGuid());
    public static ProjectId Empty() => new(Guid.Empty);

    public override string ToString() => value.ToString();
}