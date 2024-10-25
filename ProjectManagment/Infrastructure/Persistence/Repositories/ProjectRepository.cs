using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class ProjectRepository(ApplicationDbContext context) : IProjectRepository, IProjectQueries
{
    public async Task<IReadOnlyList<Project>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Projects
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Project>> GetById(ProjectId id, CancellationToken cancellationToken)
    {
        var entity = await context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.ProjectId == id, cancellationToken);
        
        return entity == null ? Option.None<Project>() : Option.Some(entity);
    }

    public async Task<Project> Create(Project project, CancellationToken cancellationToken)
    {
        await context.Projects.AddAsync(project, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return project;
    }

    public async Task<Project> Update(Project project, CancellationToken cancellationToken)
    {
        context.Projects.Update(project);
        await context.SaveChangesAsync(cancellationToken);
        
        return project;
    }

    public async Task<Project> Delete(Project project, CancellationToken cancellationToken)
    {
        context.Projects.Remove(project);
        await context.SaveChangesAsync(cancellationToken);
        
        return project;
    }
}