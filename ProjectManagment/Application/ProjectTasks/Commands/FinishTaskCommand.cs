using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.ProjectTasks.Exceptions;
using Domain.Tasks;
using Domain.Users;
using MediatR;

namespace Application.ProjectTasks.Commands;

public class FinishTaskCommand : IRequest<Result<ProjectTask, TaskException>>
{
    public required Guid TaskId { get; init; }
    public required Guid UserId { get; init; }
}

public class FinishTaskCommandHandler(ITaskRepository repository, IUserRepository userRepository) 
    : IRequestHandler<FinishTaskCommand, Result<ProjectTask, TaskException>>
{
    public async Task<Result<ProjectTask, TaskException>> Handle(FinishTaskCommand request, CancellationToken cancellationToken)
    {
        var taskId = new ProjectTaskId(request.TaskId);
        var userId = new UserId(request.UserId);
        var exsitingTask = await repository.GetById(taskId, cancellationToken);
        var exsistingUser = await userRepository.GetById(userId, cancellationToken);
        return await exsistingUser.Match(
           async u =>
            {
                return await exsitingTask.Match(
                    async t =>
                    {
                        if (t.IsFinished == true && u.Id == t.UserId || u.Role.Name == "Admin")
                        {
                            return new TaskAlreadyFinished(t.TaskId);
                        }
                        return await FinishTaskEntity(t, cancellationToken);
                    },
                    () => Task.FromResult<Result<ProjectTask, TaskException>>(new TaskNotFoundException(new ProjectTaskId(request.TaskId))));
            },()=> Task.FromResult<Result<ProjectTask, TaskException>>(new UserNotFoundWhileCreated(taskId)));
    }

    private async Task<Result<ProjectTask, TaskException>> FinishTaskEntity
        (ProjectTask projectTask,
        CancellationToken cancellationToken)
    {
        try
        {
            projectTask.FinishTask();
            
            return await repository.Update(projectTask, cancellationToken);
        }
        catch (Exception e)
        {
            return new TaskUnknowException(projectTask.TaskId, e);
        }
    }
}