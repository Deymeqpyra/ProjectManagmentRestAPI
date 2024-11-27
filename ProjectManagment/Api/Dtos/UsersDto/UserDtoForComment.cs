using Domain.Users;

namespace Api.Dtos.UsersDto;

public record UserDtoForComment(
    string Username)
{
    public static UserDtoForComment FromUser(User user)
        => new(Username: user.UserName);
}