namespace Domain.Tasks;

public record ProjectTaskId(Guid value)
{
    public static ProjectTaskId New() => new (Guid.NewGuid());
    public static ProjectTaskId Empty() => new (Guid.Empty);

    public override string ToString() => value.ToString();
}