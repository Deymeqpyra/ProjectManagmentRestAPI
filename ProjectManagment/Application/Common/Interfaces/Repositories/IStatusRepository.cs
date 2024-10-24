using Domain.Statuses;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IStatusRepository
{
    Task<Option<ProjectStatus>> GetById(ProjectStatusId id, CancellationToken cancellationToken);
    Task<ProjectStatus> Create(ProjectStatus projectStatus, CancellationToken cancellationToken);
    Task<ProjectStatus> Update(ProjectStatus projectStatus, CancellationToken cancellationToken);
    Task<ProjectStatus> Delete(ProjectStatus projectStatus, CancellationToken cancellationToken);
}