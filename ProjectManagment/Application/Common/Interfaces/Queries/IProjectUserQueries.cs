using Domain.Projects;
using Domain.ProjectUsers;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IProjectUserQueries
{
    Task<IReadOnlyList<User?>> GetUserByProject(ProjectId projectId, CancellationToken cancellationToken);

    Task<IReadOnlyList<Project?>> GetProjectByUser(UserId userId, CancellationToken cancellationToken);
    Task<Option<ProjectUser>> GetByIds(ProjectId projectId, UserId userId, CancellationToken cancellationToken);    
}