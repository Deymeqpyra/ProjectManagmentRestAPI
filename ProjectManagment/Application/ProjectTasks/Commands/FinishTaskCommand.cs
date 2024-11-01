using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.ProjectTasks.Exceptions;
using Domain.Tasks;
using MediatR;

namespace Application.ProjectTasks.Commands;

public class FinishTaskCommand : IRequest<Result<ProjectTask, TaskException>>
{
    public required Guid TaskId { get; init; }
}

public class FinishTaskCommandHandler(ITaskRepository repository) 
    : IRequestHandler<FinishTaskCommand, Result<ProjectTask, TaskException>>
{
    public async Task<Result<ProjectTask, TaskException>> Handle(FinishTaskCommand request, CancellationToken cancellationToken)
    {
        var taskId = new ProjectTaskId(request.TaskId);
        var exsitingTask = await repository.GetById(taskId, cancellationToken);
        return await exsitingTask.Match(
            async t =>
            {
                if (t.IsFinished == true)
                {
                    return new TaskAlreadyFinished(t.TaskId);
                }
                return await FinishTaskEntity(t, cancellationToken);
            },
            () => Task.FromResult<Result<ProjectTask, TaskException>>(new TaskNotFoundException(new ProjectTaskId(request.TaskId))));
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