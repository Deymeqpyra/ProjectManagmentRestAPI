using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Priorities;
using Domain.Projects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class PriorityRepository(ApplicationDbContext context) : IPriorityRepository, IPriorityQueries
{
    public async Task<Option<ProjectPriority>> GetById(ProjectPriorityId priorityId, CancellationToken cancellationToken)
    {
        var entity = await context.ProjectPriorities
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.Id == priorityId, cancellationToken);

        return entity == null ? Option.None<ProjectPriority>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<ProjectPriority>> GetAll(CancellationToken cancellationToken)
    {
        return await context.ProjectPriorities
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<ProjectPriority> Create(ProjectPriority priority, CancellationToken cancellationToken)
    {
        await context.ProjectPriorities.AddAsync(priority, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return priority;
    }

    public async Task<ProjectPriority> Update(ProjectPriority priority, CancellationToken cancellationToken)
    {
        context.ProjectPriorities.Update(priority);
        await context.SaveChangesAsync(cancellationToken);
        
        return priority;
    }

    public async Task<ProjectPriority> Delete(ProjectPriority priority, CancellationToken cancellationToken)
    {
        context.ProjectPriorities.Remove(priority);
        await context.SaveChangesAsync(cancellationToken);
        
        return priority;
    }
}