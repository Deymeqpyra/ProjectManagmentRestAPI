namespace Domain.Tasks;

public class TaskId(Guid value)
{
    public static TaskId New() => new (Guid.NewGuid());
    public static TaskId Empty() => new (Guid.Empty);

    public override string ToString() => value.ToString();
}