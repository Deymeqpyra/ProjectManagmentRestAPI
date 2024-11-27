using Domain.Projects;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IProjectQueries
{
    Task<Option<Project>> GetByIdWithStatusAndPriority(ProjectId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Project>> GetByUserId(UserId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Project>> GetAll(CancellationToken cancellationToken);
    Task<Option<Project>> GetById(ProjectId id, CancellationToken cancellationToken);
}