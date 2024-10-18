using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppilicationDbContextInitializer(ApplicationDbContext context)
{
    public async Task InitializeAsync()
    {
        await context.Database.MigrateAsync();
    }
 }