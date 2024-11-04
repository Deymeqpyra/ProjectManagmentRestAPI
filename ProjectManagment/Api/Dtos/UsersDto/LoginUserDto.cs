using Domain.Users;

namespace Api.Dtos.UsersDto;

public record LoginUserDto(string email, string password)
{
    public static LoginUserDto FromUserInfo(User user)
    => new(email: user.Email, password: user.Password);
}