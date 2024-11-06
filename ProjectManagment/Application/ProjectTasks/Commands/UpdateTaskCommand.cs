using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.ProjectTasks.Exceptions;
using Domain.Categories;
using Domain.Tasks;
using Domain.Users;
using MediatR;

namespace Application.ProjectTasks.Commands;

public class UpdateTaskCommand : IRequest<Result<ProjectTask, TaskException>>
{
    public required Guid TaskId { get; init; }
    public required string TitleUpdate { get; init; }
    public required string DescriptionUpdate { get; init; }
    public required Guid CategoryIdUpdate { get; init; }
    public required Guid UserId { get; init; }
}

public class UpdateTaskCommandHandler(ITaskRepository repository, IUserRepository userRepository)
    : IRequestHandler<UpdateTaskCommand, Result<ProjectTask, TaskException>>
{
    public async Task<Result<ProjectTask, TaskException>> Handle(UpdateTaskCommand request,
        CancellationToken cancellationToken)
    {
        var categoryId = new CategoryId(request.CategoryIdUpdate);
        var taskId = new ProjectTaskId(request.TaskId);
        var userId = new UserId(request.UserId);
        var exisitngTask = await repository.GetById(taskId, cancellationToken);
        var exitingUser = await userRepository.GetById(userId, cancellationToken);

        return await exitingUser.Match(
            async u =>
            {
                return await exisitngTask.Match(
                    async t => await UpdateEntity(t, u, request.TitleUpdate, request.DescriptionUpdate, categoryId,
                        cancellationToken),
                    () => Task.FromResult<Result<ProjectTask, TaskException>>(new TaskNotFoundException(taskId)));
            }, () => Task.FromResult<Result<ProjectTask, TaskException>>(new UserNotFoundWhileCreated(taskId)));
    }

    private async Task<Result<ProjectTask, TaskException>> UpdateEntity(
        ProjectTask task,
        User user,
        string titleUpdate,
        string descriptionUpdate,
        CategoryId categoryId,
        CancellationToken cancellationToken)
    {
        try
        {
            if (user.Id == task.UserId || user.Role.Name == "Admin")
            {
                task.UpdateDetails(titleUpdate, descriptionUpdate, categoryId);
                return await repository.Update(task, cancellationToken);
            }

            return new NotEnoughPermission(task.TaskId);
        }
        catch (Exception e)
        {
            return new TaskUnknowException(task.TaskId, e);
        }
    }
}