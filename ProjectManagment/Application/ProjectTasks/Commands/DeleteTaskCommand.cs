using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.ProjectTasks.Exceptions;
using Domain.Tasks;
using Domain.Users;
using MediatR;

namespace Application.ProjectTasks.Commands;

public class DeleteTaskCommand : IRequest<Result<ProjectTask, TaskException>>
{
    public required Guid TaskId { get; init; }
    public required Guid UserId { get; init; }
}

public class DeleteTaskCommandHandler(ITaskRepository repository, IUserRepository userRepository)
    : IRequestHandler<DeleteTaskCommand, Result<ProjectTask, TaskException>>
{
    public async Task<Result<ProjectTask, TaskException>> Handle(DeleteTaskCommand request,
        CancellationToken cancellationToken)
    {
        var exisitingTask = await repository.GetById(new ProjectTaskId(request.TaskId), cancellationToken);
        var exsistingUser = await userRepository.GetById(new UserId(request.UserId), cancellationToken);
        return await exsistingUser.Match(
            async u =>
            {
                return await exisitingTask.Match(
                    async t => await DeleteEntity(t, u, cancellationToken),
                    () => Task.FromResult<Result<ProjectTask, TaskException>>(
                        new TaskNotFoundException(new ProjectTaskId(request.TaskId))));
            },
            () => Task.FromResult<Result<ProjectTask, TaskException>>(
                new UserNotFoundWhileCreated(new ProjectTaskId(request.TaskId)))
        );
    }

    private async Task<Result<ProjectTask, TaskException>> DeleteEntity(
        ProjectTask task,
        User user,
        CancellationToken cancellationToken)
    {
        try
        {
            if (user.Id == task.UserId || user.Role!.Name == "Admin")
            {
                return await repository.Delete(task, cancellationToken);
            }

            return new NotEnoughPermission(task.TaskId);
        }
        catch (Exception e)
        {
            return new TaskUnknowException(task.TaskId, e);
        }
    }
}