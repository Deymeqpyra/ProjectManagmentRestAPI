namespace Domain.Roles;

public class Role
{
    public RoleId Id { get; }

    public string Name { get; private set; }

    private Role(RoleId id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Role New(RoleId id, string name)
        => new(id, name);

    public void UpdateDetails(string updatedName)
    {
        Name = updatedName;
    }
}