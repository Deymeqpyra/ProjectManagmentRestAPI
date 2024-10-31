using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Statuses;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class StatusRepository(ApplicationDbContext context) : IStatusRepository, IStatusQueries
{
    public async Task<IReadOnlyList<ProjectStatus>> GetAll(CancellationToken cancellationToken)
    {
        return await context.ProjectStatuses
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<ProjectStatus>> GetById(ProjectStatusId id, CancellationToken cancellationToken)
    {
        var entity = await context.ProjectStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity == null ? Option.None<ProjectStatus>() : Option.Some(entity);
    }
    public async Task<Option<ProjectStatus>> GetByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.ProjectStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name);

        return entity == null ? Option.None<ProjectStatus>() : Option.Some(entity);
    }

    public async Task<ProjectStatus> Create(ProjectStatus projectStatus, CancellationToken cancellationToken)
    {
        await context.ProjectStatuses.AddAsync(projectStatus, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return projectStatus;
    }

    public async Task<ProjectStatus> Update(ProjectStatus projectStatus, CancellationToken cancellationToken)
    {
        context.ProjectStatuses.Update(projectStatus);
        await context.SaveChangesAsync(cancellationToken);

        return projectStatus;
    }

    public async Task<ProjectStatus> Delete(ProjectStatus projectStatus, CancellationToken cancellationToken)
    {
        context.ProjectStatuses.Remove(projectStatus);
        await context.SaveChangesAsync(cancellationToken);

        return projectStatus;
    }
}