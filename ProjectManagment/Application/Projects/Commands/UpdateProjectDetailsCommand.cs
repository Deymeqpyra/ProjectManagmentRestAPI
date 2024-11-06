using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Projects.Exceptions;
using Domain.Projects;
using Domain.Statuses;
using Domain.Users;
using MediatR;

namespace Application.Projects.Commands;

public class UpdateProjectDetailsCommand : IRequest<Result<Project, ProjectException>>
{
    public required Guid ProjectId { get; init; }
    public required string UpdateTitle { get; init; }
    public required string UpdateDescription { get; init; }
    public required Guid UserId { get; init; }
}

public class UpdateProjectDetailsCommandHandler(IProjectRepository repository, IUserRepository userRepository)
    : IRequestHandler<UpdateProjectDetailsCommand, Result<Project, ProjectException>>
{
    public async Task<Result<Project, ProjectException>> Handle(UpdateProjectDetailsCommand request,
        CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var userId = new UserId(request.UserId);
        var exsitingProject = await repository.GetById(projectId, cancellationToken);
        var exsitingUser = await userRepository.GetById(userId, cancellationToken);

        return await exsitingUser.Match(
            async u =>
            {
                return await exsitingProject.Match(
                    async p => await UpdateEntity(p, u, request.UpdateTitle, request.UpdateDescription,
                        cancellationToken),
                    () => Task.FromResult<Result<Project, ProjectException>>(new ProjectNotFound(projectId))
                );
            },
            () => Task.FromResult<Result<Project, ProjectException>>(new UserNotFoundWhileCreated(projectId)));
    }

    private async Task<Result<Project, ProjectException>> UpdateEntity(
        Project project,
        User user,
        string updateTitle,
        string updateDescription,
        CancellationToken cancellationToken)
    {
        try
        {
            if (user.Id == project.UserId || user.Role.Name == "Admin")
            {
                project.UpdateDetails(updateTitle, updateDescription);

                return await repository.Update(project, cancellationToken);
            }

            return new NotEnoughPermission(project.ProjectId);

        }
        catch (Exception e)
        {
            return new ProjectUnknownException(project.ProjectId, e);
        }
    }
}