using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Projects;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class TaskRepository(ApplicationDbContext context) : ITaskRepository, ITaskQueries
{
    public async Task<IReadOnlyList<ProjectTask>> GetAll(CancellationToken cancellationToken)
    {
        return await context.ProjectTasks
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<ProjectTask>> GetById(ProjectTaskId id, CancellationToken cancellationToken)
    {
        var entity = await context.ProjectTasks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TaskId == id, cancellationToken);

        return entity == null ? Option.None<ProjectTask>() : Option.Some(entity);
    }
    public async Task<Option<ProjectTask>> GetByTitle(string title, CancellationToken cancellationToken)
    {
        var entity = await context.ProjectTasks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Title == title, cancellationToken);

        return entity == null ? Option.None<ProjectTask>() : Option.Some(entity);
    }
    public async Task<IReadOnlyList<ProjectTask>> GetByProjectId(ProjectId projectId, CancellationToken cancellationToken)
    {
        return await context.ProjectTasks
            .Include(x=>x.Category)
            .Include(x=>x.User)
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }
    public async Task<ProjectTask> Create(ProjectTask projectTask, CancellationToken cancellationToken)
    {
        await context.ProjectTasks.AddAsync(projectTask, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return projectTask;
    }

    public async Task<ProjectTask> Update(ProjectTask projectTask, CancellationToken cancellationToken)
    {
        context.ProjectTasks.Update(projectTask);
        await context.SaveChangesAsync(cancellationToken);

        return projectTask;
    }

    public async Task<ProjectTask> Delete(ProjectTask projectTask, CancellationToken cancellationToken)
    {
        context.ProjectTasks.Remove(projectTask);
        await context.SaveChangesAsync(cancellationToken);
        
        return projectTask;
    }
}