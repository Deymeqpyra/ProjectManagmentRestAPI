using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Projects;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class ProjectRepository(ApplicationDbContext context) : IProjectRepository, IProjectQueries
{
    public async Task<IReadOnlyList<Project>> GetByUserId(UserId id, CancellationToken cancellationToken)
    {
        return await context.Projects
            .Where(p => p.UserId == id)
            .Include(x => x.TagsProjects)
            .ThenInclude(x => x.Tag)
            .Include(x => x.ProjectUsers)
            .ThenInclude(x => x.User)
            .ThenInclude(x => x.ProjectTask)
            .Include(x => x.ProjectPriority)
            .Include(x => x.ProjectStatus)
            .Include(x => x.Comments)
            .ThenInclude(x => x.User)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Project>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Projects
            .Include(x => x.TagsProjects)
            .ThenInclude(x => x.Tag)
            .Include(x => x.ProjectUsers)
            .ThenInclude(x => x.User)
            .ThenInclude(x => x.ProjectTask)
            .Include(x => x.ProjectPriority)
            .Include(x => x.ProjectStatus)
            .Include(x => x.Comments)
            .ThenInclude(x => x.User)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Project>> GetByIdWithStatusAndPriority(ProjectId id, CancellationToken cancellationToken)
    {
        var entity = await context.Projects
            .Include(x => x.ProjectPriority)
            .Include(x => x.ProjectStatus)
            .Include(x => x.Comments)
            .ThenInclude(x => x.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProjectId == id, cancellationToken);

        return entity == null ? Option.None<Project>() : Option.Some(entity);
    }

    public async Task<Option<Project>> GetById(ProjectId id, CancellationToken cancellationToken)
    {
        var entity = await context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProjectId == id, cancellationToken);

        return entity == null ? Option.None<Project>() : Option.Some(entity);
    }

    public async Task<Option<Project>> GetByTitle(string title, CancellationToken cancellationToken)
    {
        var entity = await context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Title == title, cancellationToken);

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