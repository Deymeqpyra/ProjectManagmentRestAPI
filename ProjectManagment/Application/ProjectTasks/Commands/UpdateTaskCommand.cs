using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.ProjectTasks.Exceptions;
using Domain.Categories;
using Domain.Tasks;
using MediatR;

namespace Application.ProjectTasks.Commands;

public class UpdateTaskCommand : IRequest<Result<ProjectTask, TaskException>>
{
    public required Guid TaskId { get; init; }
    public required string TitleUpdate { get; init; }
    public required string DescriptionUpdate { get; init; }
    public required Guid CategoryIdUpdate { get; init; }
}
public class UpdateTaskCommandHandler(ITaskRepository repository) : IRequestHandler<UpdateTaskCommand, Result<ProjectTask, TaskException>>
{
    public async Task<Result<ProjectTask, TaskException>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var categoryId = new CategoryId(request.CategoryIdUpdate);
        var taskId= new ProjectTaskId(request.TaskId);
        var exisitngTask = await repository.GetById(taskId, cancellationToken);
        
        return await exisitngTask.Match(
            async t=> await UpdateEntity(t, request.TitleUpdate, request.DescriptionUpdate, categoryId, cancellationToken),
            () => Task.FromResult<Result<ProjectTask, TaskException>>(new TaskNotFoundException(taskId)));
    }

    private async Task<Result<ProjectTask, TaskException>> UpdateEntity(
        ProjectTask task,
        string titleUpdate,
        string descriptionUpdate,
        CategoryId categoryId,
        CancellationToken cancellationToken)
    {
        try
        {
            task.UpdateDetails(titleUpdate, descriptionUpdate, categoryId);
            
            return await repository.Update(task, cancellationToken);
        }
        catch (Exception e)
        {
            return new TaskUnknowException(task.TaskId, e);
        }
    }
}