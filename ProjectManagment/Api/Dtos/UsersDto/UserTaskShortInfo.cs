using Domain.Users;

namespace Api.Dtos.UsersDto;

public record UserTaskShortInfo(
    Guid UserId,
    string Username)
{
    public static UserTaskShortInfo FromUser(User user)
        => new(UserId: user.Id.value,
            Username: user.UserName);
}