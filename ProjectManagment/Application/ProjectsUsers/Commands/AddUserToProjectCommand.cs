using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.ProjectsUsers.Exceptions;
using Domain.Projects;
using Domain.ProjectUsers;
using Domain.Users;
using MediatR;

namespace Application.ProjectsUsers.Commands;

public class AddUserToProjectCommand : IRequest<Result<ProjectUser, ProjectUserException>>
{
    public required Guid ProjectId { get; init; }
    public required Guid UserIdWhoCreated { get; init; }
    public required Guid UserId { get; init; }
}

public class AddUserToProjectCommandHandler(
    IProjectUserRepository repository,
    IProjectRepository projectRepository,
    IUserRepository userRepository)
    : IRequestHandler<AddUserToProjectCommand, Result<ProjectUser, ProjectUserException>>
{
    public async Task<Result<ProjectUser, ProjectUserException>> Handle(AddUserToProjectCommand request,
        CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var userId = new UserId(request.UserId);
        var userIdWhoCreated = new UserId(request.UserIdWhoCreated);

        var exstingProjectUser = await repository.GetByIds(projectId, userId, cancellationToken);

        return await exstingProjectUser.Match(
            pu => Task.FromResult<Result<ProjectUser, ProjectUserException>>(
                new UserAlreadyInProject(projectId, userId)),
            async () =>
            {
                var exsitingProject = await projectRepository.GetById(projectId, cancellationToken);
                return await exsitingProject.Match(
                    async p =>
                    {
                        var exsitingUserWhoCreate = await userRepository.GetById(userIdWhoCreated, cancellationToken);
                        return await exsitingUserWhoCreate.Match(
                            async exu =>
                            {
                                var exsitingUser = await userRepository.GetById(userId, cancellationToken);
                                return await exsitingUser.Match(
                                    async u =>
                                    {
                                        if (p.UserId.value == userIdWhoCreated.value || exu.Role!.Name == "Admin")
                                        {
                                            return await AddAsync(p.ProjectId, u.Id, cancellationToken);
                                        }

                                        return await Task.FromResult<Result<ProjectUser, ProjectUserException>>(
                                            new UserNotEnoughPremission(projectId, userIdWhoCreated));
                                    },
                                    () => Task.FromResult<Result<ProjectUser, ProjectUserException>>(
                                        new UserNotFound(projectId, userId)));
                            },
                            () => Task.FromResult<Result<ProjectUser, ProjectUserException>>(
                                new UserNotFound(projectId, userId)));
                    },
                    () => Task.FromResult<Result<ProjectUser, ProjectUserException>>(
                        new ProjectNotFound(projectId, userId)));
            });
    }

    private async Task<Result<ProjectUser, ProjectUserException>> AddAsync(
        ProjectId projectId,
        UserId userId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = ProjectUser.New(projectId, userId);

            return await repository.Create(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new ProjectUserUnknownException(projectId, userId, e);
        }
    }
}