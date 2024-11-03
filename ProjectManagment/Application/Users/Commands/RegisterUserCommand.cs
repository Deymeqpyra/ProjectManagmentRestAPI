using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Roles;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public class RegisterUserCommand : IRequest<Result<User, UserException>>
{
    public string Username { get; init; }
    public string Password { get; init; }
    public string Email { get; init; }
}

public class RegisterUserCommandHandler(IUserRepository repository, IRoleRepository roleRepository)
    : IRequestHandler<RegisterUserCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var userRole = "User";
        var exsitingRole = await roleRepository.GetByName(userRole, cancellationToken);
        var exsitingUser = await repository.GetByEmail(request.Email, cancellationToken);

        return await exsitingUser.Match(
            u => Task.FromResult<Result<User, UserException>>(new UserAlreadyExistsException(u.Id)),
            async () =>
            {
                return await exsitingRole.Match(
                    async r => await CreateEntity(UserId.New(), request.Username, request.Password, request.Email, r.Id,
                        cancellationToken),
                    () => Task.FromResult<Result<User, UserException>>(new RoleNotFound(UserId.Empty(),
                        RoleId.Empty())));
            });
    }

    private async Task<Result<User, UserException>> CreateEntity(
        UserId userId,
        string userName,
        string password,
        string email,
        RoleId roleId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = User.RegisterNewUser(userId, userName, password, email, roleId);

            return await repository.Create(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new UserUnknownException(UserId.Empty(), e);
        }
    }
}