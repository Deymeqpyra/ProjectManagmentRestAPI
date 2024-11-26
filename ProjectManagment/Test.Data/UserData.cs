using Domain.Roles;
using Domain.Users;

namespace Test.Data;

public static class UserData
{
    public static User AdminUser(RoleId roleId)
        => User.RegisterNewUser(UserId.New(), "admin", "admin", "admin@admin.com", roleId);
    public static User ExtraUser(RoleId roleId)
        => User.RegisterNewUser(UserId.New(), "user", "user", "user@user.com", roleId);
}