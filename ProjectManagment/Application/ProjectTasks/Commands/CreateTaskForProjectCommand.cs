using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.ProjectTasks.Exceptions;
using Domain.Categories;
using Domain.Projects;
using Domain.Tasks;
using Domain.Users;
using MediatR;

namespace Application.ProjectTasks.Commands;

public class CreateTaskForProjectCommand : IRequest<Result<ProjectTask, TaskException>>
{
    public required string TaskTitle { get; init; }
    public required string ShortDescription { get; init; }
    public required Guid ProjectId { get; init; }
    public required Guid CategoryId { get; init; }
    public required Guid UserId { get; init; }
}

public class CreateTaskForProjectCommandHandler(ITaskRepository repository, IUserRepository userRepository) :
    IRequestHandler<CreateTaskForProjectCommand, Result<ProjectTask, TaskException>>
{
    public async Task<Result<ProjectTask, TaskException>> Handle(CreateTaskForProjectCommand request,
        CancellationToken cancellationToken)
    {
        bool isFinishedTask = false;
        var existingTask = await repository.GetByTitle(request.TaskTitle, cancellationToken);
        var user = await userRepository.GetById(new UserId(request.UserId), cancellationToken);
        return await user.Match(
            async u =>
            {
                return await existingTask.Match(
                    t => Task.FromResult<Result<ProjectTask, TaskException>>(new TaskAlreadyExistsException(t.TaskId)),
                    async () => await CreateEntity(
                        request.TaskTitle,
                        request.ShortDescription,
                        isFinishedTask,
                        u,
                        new ProjectId(request.ProjectId),
                        new CategoryId(request.CategoryId),
                        cancellationToken));
            },
            () => Task.FromResult<Result<ProjectTask, TaskException>>(
                new UserNotFoundWhileCreated(ProjectTaskId.Empty())));
    }

    private async Task<Result<ProjectTask, TaskException>> CreateEntity(
        string taskTitle,
        string shortDescription,
        bool isFinished,
        User user,
        ProjectId projectId,
        CategoryId categoryId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = ProjectTask.New(ProjectTaskId.New(),taskTitle, shortDescription, isFinished, user.Id, projectId, categoryId);

            return await repository.Create(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new TaskUnknowException(ProjectTaskId.Empty(), e);
        }
    }
}