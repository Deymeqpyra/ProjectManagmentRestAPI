namespace Domain.Users;

public record UserId(Guid value)
{
    public static UserId New() => new (Guid.NewGuid());
    public static UserId Empty() => new (Guid.Empty);

    public override string ToString() => value.ToString();
}