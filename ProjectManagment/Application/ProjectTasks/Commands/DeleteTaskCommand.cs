using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.ProjectTasks.Exceptions;
using Domain.Tasks;
using MediatR;

namespace Application.ProjectTasks.Commands;

public class DeleteTaskCommand : IRequest<Result<ProjectTask, TaskException>>
{
    public required Guid TaskId { get; init; }
}

public class DeleteTaskCommandHandler(ITaskRepository repository)
    : IRequestHandler<DeleteTaskCommand, Result<ProjectTask, TaskException>>
{
    public async Task<Result<ProjectTask, TaskException>> Handle(DeleteTaskCommand request,
        CancellationToken cancellationToken)
    {
        var exisitingTask = await repository.GetById(new ProjectTaskId(request.TaskId), cancellationToken);

        return await exisitingTask.Match(
            async t => await DeleteEntity(t, cancellationToken),
            () => Task.FromResult<Result<ProjectTask, TaskException>>(
                new TaskNotFoundException(new ProjectTaskId(request.TaskId))));
    }

    private async Task<Result<ProjectTask, TaskException>> DeleteEntity(
        ProjectTask task,
        CancellationToken cancellationToken)
    {
        try
        {
            return await repository.Delete(task, cancellationToken);
        }
        catch (Exception e)
        {
            return new TaskUnknowException(task.ProjectTaskId, e);
        }
    }
}