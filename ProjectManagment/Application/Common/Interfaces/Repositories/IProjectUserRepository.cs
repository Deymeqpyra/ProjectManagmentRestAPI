using Domain.ProjectUsers;

namespace Application.Common.Interfaces.Repositories;

public interface IProjectUserRepository
{
    Task<ProjectUser> Create(ProjectUser projectUser, CancellationToken cancellationToken);
    Task<ProjectUser> Update(ProjectUser projectUser, CancellationToken cancellationToken);
    Task<ProjectUser> Delete(ProjectUser projectUser, CancellationToken cancellationToken);
}