using Domain.Projects;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IProjectQueries
{
    Task<IReadOnlyList<Project>> GetAll(CancellationToken cancellationToken);
    Task<Option<Project>> GetById(ProjectId id, CancellationToken cancellationToken);
}