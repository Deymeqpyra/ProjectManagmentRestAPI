using Domain.Priorities;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IPriorityQueries
{
    Task<IReadOnlyList<ProjectPriority>> GetAll(CancellationToken cancellationToken);
    Task<Option<ProjectPriority>> GetById(ProjectPriorityId priorityId, CancellationToken cancellationToken);

}