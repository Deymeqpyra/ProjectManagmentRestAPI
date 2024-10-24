using Domain.Statuses;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IStatusQueries
{
    Task<Option<ProjectStatus>> GetById(ProjectStatusId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<ProjectStatus>> GetAll(CancellationToken cancellationToken);
}