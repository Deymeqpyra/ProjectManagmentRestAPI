using Domain.Tasks;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<Option<ProjectTask>> GetByTitle(string title, CancellationToken cancellationToken);
    Task<Option<ProjectTask>> GetById(ProjectTaskId id, CancellationToken cancellationToken);
    Task<ProjectTask> Create(ProjectTask projectTask, CancellationToken cancellationToken);
    Task<ProjectTask> Update(ProjectTask projectTask, CancellationToken cancellationToken);
    Task<ProjectTask> Delete(ProjectTask projectTask, CancellationToken cancellationToken);
}