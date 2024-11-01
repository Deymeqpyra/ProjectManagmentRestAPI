using Domain.Projects;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<Option<Project>> GetByTitle(string title, CancellationToken cancellationToken);
    Task<Option<Project>> GetById(ProjectId id, CancellationToken cancellationToken);
    Task<Project> Create(Project project, CancellationToken cancellationToken);
    Task<Project> Update(Project project, CancellationToken cancellationToken);
    Task<Project> Delete(Project project, CancellationToken cancellationToken);
}