using Domain.Tasks;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ITaskQueries
{
    Task<IReadOnlyList<ProjectTask>> GetAll(CancellationToken cancellationToken);
    Task<Option<ProjectTask>> GetById(ProjectTaskId id, CancellationToken cancellationToken);
}