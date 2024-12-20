using System.Xml.Schema;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository, IUserQueries
{
    public async Task<IReadOnlyList<User>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .Include(x=>x.ProjectTask)
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<User>> GetByIdWithRoles(UserId id, CancellationToken cancellationToken)
    {
        var entity = await context.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .Include(x=>x.ProjectTask)
            .FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
        
        return entity == null ? Option.None<User>() : Option.Some(entity);
    }
    public async Task<Option<User>> GetById(UserId id, CancellationToken cancellationToken)
    {
        var entity = await context.Users
            .Include(x=>x.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
        
        return entity == null ? Option.None<User>() : Option.Some(entity);
    }
    public async Task<Option<User>> GetByEmail(string email, CancellationToken cancellationToken)
    {
        var entity = await context.Users
            .Include(x=>x.Role)
            .Include(x=>x.ProjectUsers)
                .ThenInclude(x=>x.Project)
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.Email == email, cancellationToken);
        
        return entity == null ? Option.None<User>() : Option.Some(entity);
    }
    public async Task<Option<User>> GetByEmailAndPassword(string email, string password, CancellationToken cancellationToken)
    {
        var entity = await context.Users
            .Include(x=>x.Role)
            .Include(x=>x.ProjectUsers)
                .ThenInclude(x=>x.Project)
            .AsNoTracking()
            .SingleOrDefaultAsync(x=>x.Email == email & x.Password == password, cancellationToken);
        
        return entity == null ? Option.None<User>() : Option.Some(entity);
    }
    public async Task<User> Create(User user, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<User> Update(User user, CancellationToken cancellationToken)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<User> Delete(User user, CancellationToken cancellationToken)
    {
        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);

        return user;
    }
}