using Domain.Users;

namespace Api.Dtos.UsersDto;

public record CreateUserDto
(
    string UserName,
    string Email,
    string Password)
{
    public static CreateUserDto FromUser(User user)
    => new(
        UserName: user.UserName,
        Email: user.Email,
        Password: user.Password);
}