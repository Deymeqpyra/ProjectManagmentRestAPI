using Domain.Roles;

namespace Test.Data;

public static class RoleData
{
    public static Role AdminRole()
    => Role.New(RoleId.New(), "Admin");
    public static Role ExtraRole()
        => Role.New(RoleId.New(), "ExtraRole");
}