using Domain.Priorities;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IPriorityRepository
{
    Task<Option<ProjectPriority>> GetByName(string Name, CancellationToken cancellationToken);
    Task<Option<ProjectPriority>> GetById(ProjectPriorityId priorityId, CancellationToken cancellationToken);
    Task<ProjectPriority> Update(ProjectPriority priority, CancellationToken cancellationToken);
    Task<ProjectPriority> Create(ProjectPriority priority, CancellationToken cancellationToken);
    Task<ProjectPriority> Delete(ProjectPriority priority, CancellationToken cancellationToken);
}