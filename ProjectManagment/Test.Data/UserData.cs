using DevOne.Security.Cryptography.BCrypt;
using Domain.Roles;
using Domain.Users;

namespace Test.Data;

public static class UserData
{
    public const string passwordAdmin = "admin";
    public const string passwordUser = "user";
    public static User AdminUser(RoleId roleId)
        => User.RegisterNewUser(UserId.New(), "admin", passwordAdmin, "admin@admin.com", roleId);
    public static User ExtraUser(RoleId roleId)
        => User.RegisterNewUserWithoutCryption(UserId.New(), "user", passwordUser, "user@user.com", roleId);
}