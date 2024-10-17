namespace Domain.Tags;

public class TagId(Guid value)
{
    public static TagId New() => new (Guid.NewGuid());
    public static TagId Empty() => new (Guid.Empty);

    public override string ToString() => value.ToString();
}