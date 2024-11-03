using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public class UpdateUserNameCommand : IRequest<Result<User, UserException>>
{
    public required Guid UserId { get; init; }
    public required string UserName { get; init; }
}

public class UpdateUserNameCommandHandler(IUserRepository repository)
    : IRequestHandler<UpdateUserNameCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(UpdateUserNameCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var exsitingUser = await repository.GetById(userId, cancellationToken);

        return await exsitingUser.Match(
            async u => await UpdateEntity(u, request.UserName, cancellationToken),
            () => Task.FromResult<Result<User, UserException>>(new UserNotFoundException(userId)));
    }

    private async Task<Result<User, UserException>> UpdateEntity(
        User user,
        string userName,
        CancellationToken cancellationToken)
    {
        try
        {
            user.UpdateUserName(userName);

            return await repository.Update(user, cancellationToken);
        }
        catch (Exception e)
        {
            return new UserUnknownException(user.Id, e);
        }
    }
}