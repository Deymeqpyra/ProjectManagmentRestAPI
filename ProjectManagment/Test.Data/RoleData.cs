using Domain.Roles;

namespace Test.Data;

public static class RoleData
{
    public static Role AdminRole()
    => Role.New(RoleId.New(), "Admin");
}