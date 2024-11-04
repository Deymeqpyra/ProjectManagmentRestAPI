using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Projects;
using Domain.Tags;
using Domain.TagsProjects;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class TagProjectRepository(ApplicationDbContext context) : ITagProjectRepository, ITagProjectQueries
{
    public async Task<IReadOnlyList<TagsProject>> GetByProjectId(ProjectId projectId, CancellationToken cancellationToken)
    {
        return await context.TagsProjects
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.Tag)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<TagsProject>> GetByTagId(TagId tagId, CancellationToken cancellationToken)
    {
        return await context.TagsProjects
            .Where(t => t.TagId == tagId)
            .Include(t => t.Tag)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public async Task<Option<TagsProject>> GetByTagAndProjectId(TagId tagId, ProjectId projectId, CancellationToken cancellationToken)
    {
        var entity = await context.TagsProjects
            .AsNoTracking()
            .Include(t => t.Tag)
            .FirstOrDefaultAsync(x=>x.TagId == tagId && x.ProjectId == projectId, cancellationToken);
        
        return entity == null ? Option.None<TagsProject>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<TagsProject>> GetByAll(CancellationToken cancellationToken)
    {
        return await context.TagsProjects
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public async Task<TagsProject> Create(TagsProject tagProject, CancellationToken cancellationToken)
    {
        await context.TagsProjects.AddAsync(tagProject, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
     
        return tagProject;
    }

    public async Task<TagsProject> Update(TagsProject tagProject, CancellationToken cancellationToken)
    {
        context.TagsProjects.Update(tagProject);
        await context.SaveChangesAsync(cancellationToken);
        
        return tagProject;
    }

    public async Task<TagsProject> Delete(TagsProject tagProject, CancellationToken cancellationToken)
    {
        context.TagsProjects.Remove(tagProject);
        await context.SaveChangesAsync(cancellationToken);
        
        return tagProject;
    }
}