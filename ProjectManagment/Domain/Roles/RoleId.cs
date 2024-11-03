namespace Domain.Roles;

public record RoleId(Guid value)
{
    public static RoleId New() => new (Guid.NewGuid());
    public static RoleId Empty() => new (Guid.Empty);

    public override string ToString() => value.ToString();
}