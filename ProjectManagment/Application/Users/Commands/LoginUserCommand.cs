using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Users;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Application.Users.Commands;

public class LoginUserCommand : IRequest<Result<string, UserException>>
{
    public required string Email { get; init; }
    public required string PassWord { get; init; }
}

public class LoginUserCommandHandler(IUserRepository repository)
    : IRequestHandler<LoginUserCommand, Result<string, UserException>>
{
    public async Task<Result<string, UserException>> Handle(LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await repository.GetByEmailAndPassword(request.Email, request.PassWord, cancellationToken);

        return await user.Match(
             u =>  GenerateToken(u),
            () => Task.FromResult<Result<string, UserException>>(new WrongCrenditals(UserId.Empty())));
    }

    private async Task<Result<string, UserException>> GenerateToken(User user)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = "ForTheLoveOfGodStoreAndLoadThisSecurely"u8.ToArray();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(ClaimTypes.Role, user.Role!.Name)
            };

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = "http://localhost:5134",
                Audience = "http://localhost:5134",

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception e)
        {
            return new UserUnknownException(user.Id, e);
        }
    }
}