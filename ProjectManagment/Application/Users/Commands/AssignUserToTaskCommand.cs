using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Tasks;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public class AssignUserToTaskCommand : IRequest<Result<User, UserException>>
{
    public required Guid UserId { get; init; }
    public required Guid TaskId { get; init; }
}
public class AssignUserToTaskCommandHandler(IUserRepository repository, ITaskRepository taskRepository) : IRequestHandler<AssignUserToTaskCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(AssignUserToTaskCommand request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var taskId = new ProjectTaskId(request.TaskId);
        var exisitingUser = await repository.GetById(userId, cancellationToken);
        var exisitingTask = await taskRepository.GetById(taskId, cancellationToken);

        return await exisitingUser.Match(
            async u =>
            {
                return await exisitingTask.Match(
                    async t => await Assign(u, t.TaskId, cancellationToken),
                    () => Task.FromResult<Result<User, UserException>>(new TaskNotFound(userId, taskId)));
            },
            () => Task.FromResult<Result<User, UserException>>(new UserNotFoundException(userId)));
    }

    private async Task<Result<User, UserException>> Assign(User user, ProjectTaskId taskId,
        CancellationToken cancellationToken)
    {
        try
        {
            user.AssignProjectTask(taskId);
            
            return await repository.Update(user, cancellationToken);
        }
        catch (Exception e)
        {
            return new UserUnknownException(user.Id, e);
        }
    }
}