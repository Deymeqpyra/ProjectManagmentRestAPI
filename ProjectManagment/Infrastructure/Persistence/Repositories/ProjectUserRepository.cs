using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Projects;
using Domain.ProjectUsers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class ProjectUserRepository(ApplicationDbContext context) : IProjectUserRepository, IProjectUserQueries
{
    public async Task<IReadOnlyList<User?>> GetUserByProject(
        ProjectId projectId,
        CancellationToken cancellationToken)
    {
      return await context.ProjectUsers
            .AsNoTracking()
            .Where(p => p.ProjectId == projectId)
            .Select(x=>x.User)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Project?>> GetProjectByUser(UserId userId, CancellationToken cancellationToken)
    {
        return await context.ProjectUsers
            .AsNoTracking()
            .Where(x=>x.UserId == userId)
            .Select(x=>x.Project)
            .ToListAsync();
    }

    public async Task<Option<ProjectUser>> GetByIds(ProjectId projectId, UserId userId, CancellationToken cancellationToken)
    {
        var entity = await context.ProjectUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.ProjectId == projectId && x.UserId == userId);
        
        return entity == null ? Option.None<ProjectUser>() : Option.Some(entity);
    }

    public async Task<ProjectUser> Create(ProjectUser projectUser, CancellationToken cancellationToken)
    {
        await context.ProjectUsers.AddAsync(projectUser, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return projectUser;
    }

    public async Task<ProjectUser> Update(ProjectUser projectUser, CancellationToken cancellationToken)
    {
        context.ProjectUsers.Update(projectUser);
        await context.SaveChangesAsync(cancellationToken);

        return projectUser;
    }

    public async Task<ProjectUser> Delete(ProjectUser projectUser, CancellationToken cancellationToken)
    {
        context.ProjectUsers.Remove(projectUser);
        await context.SaveChangesAsync(cancellationToken);

        return projectUser;
    }
}