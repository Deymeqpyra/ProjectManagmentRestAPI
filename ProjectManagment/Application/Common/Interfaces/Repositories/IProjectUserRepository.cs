using Domain.Projects;
using Domain.ProjectUsers;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IProjectUserRepository
{
    Task<Option<ProjectUser>> GetByIds(ProjectId projectId, UserId userId, CancellationToken cancellationToken);
    Task<ProjectUser> Create(ProjectUser projectUser, CancellationToken cancellationToken);
    Task<ProjectUser> Update(ProjectUser projectUser, CancellationToken cancellationToken);
    Task<ProjectUser> Delete(ProjectUser projectUser, CancellationToken cancellationToken);
}