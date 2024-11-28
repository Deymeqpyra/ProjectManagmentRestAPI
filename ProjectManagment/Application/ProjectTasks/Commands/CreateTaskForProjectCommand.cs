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

public class CreateTaskForProjectCommandHandler(
    ITaskRepository repository,
    ICategoryRepository categoryRepository,
    IProjectRepository projectRepository,
    IUserRepository userRepository) :
    IRequestHandler<CreateTaskForProjectCommand, Result<ProjectTask, TaskException>>
{
    public async Task<Result<ProjectTask, TaskException>> Handle(CreateTaskForProjectCommand request,
        CancellationToken cancellationToken)
    {
        bool isFinishedTask = false;
        var categoryId = new CategoryId(request.CategoryId);
        var projectId = new ProjectId(request.ProjectId);
        var userId = new UserId(request.UserId);

        var existingTask = await repository.GetByTitle(request.TaskTitle, cancellationToken);
        var exisitingProject = await projectRepository.GetById(projectId, cancellationToken);
        var existingCategory = await categoryRepository.GetById(categoryId, cancellationToken);
        var user = await userRepository.GetById(userId, cancellationToken);
        return await existingCategory.Match(
            async category =>
            {
                return await exisitingProject.Match(
                    async project =>
                    {
                        return await user.Match(
                            async user =>
                            {
                                return await existingTask.Match(
                                    t => Task.FromResult<Result<ProjectTask, TaskException>>(
                                        new TaskAlreadyExistsException(t.TaskId)),
                                    async () => await CreateEntity(
                                        request.TaskTitle,
                                        request.ShortDescription,
                                        isFinishedTask,
                                        user,
                                        project,
                                        category,
                                        cancellationToken));
                            },
                            () => Task.FromResult<Result<ProjectTask, TaskException>>(
                                new UserNotFoundWhileCreated(ProjectTaskId.Empty())));
                    },
                    () => Task.FromResult<Result<ProjectTask, TaskException>>(
                        new UserNotFoundWhileCreated(ProjectTaskId.Empty())));
            },
            () => Task.FromResult<Result<ProjectTask, TaskException>>(new CategoryNotFound(
                ProjectTaskId.Empty(),
                categoryId))
        );
    }

    private async Task<Result<ProjectTask, TaskException>> CreateEntity(
        string taskTitle,
        string shortDescription,
        bool isFinished,
        User user,
        Project project,
        Category category,
        CancellationToken cancellationToken)
    {
        try
        {
            if (project.UserId == user.Id || user.Role!.Name == "Admin")
            {
                var entity = ProjectTask.New(ProjectTaskId.New(), taskTitle, shortDescription, isFinished, user.Id,
                    project.ProjectId, category.Id);

                return await repository.Create(entity, cancellationToken);
            }

            return await Task.FromResult<Result<ProjectTask, TaskException>>(
                new UserNotEnoughPremission(ProjectTaskId.Empty()));
        }
        catch (Exception e)
        {
            return new TaskUnknowException(ProjectTaskId.Empty(), e);
        }
    }
}