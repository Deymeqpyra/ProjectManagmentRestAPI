namespace Domain.Priorities;

public record ProjectPriorityId(Guid value)
{
    public static ProjectPriorityId New() => new(Guid.NewGuid());
    public static ProjectPriorityId Empty() => new(Guid.Empty);

    public override string ToString() => value.ToString();
}