using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Persistence;

public static class ConfigurePersistence
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuild = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Default"));
        dataSourceBuild.EnableDynamicJson();

        var dataSource = dataSourceBuild.Build();

        services.AddDbContext<ApplicationDbContext>(
            options=>options
                .UseNpgsql(dataSource,
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .ConfigureWarnings(w=>w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

        services.AddScoped<ApplicationDbContextInitializer>();
        services.AddRepositories();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        services.AddScoped<IUserRepository>(provider => provider.GetRequiredService<UserRepository>());
        services.AddScoped<IUserQueries>(provider => provider.GetRequiredService<UserRepository>());
        
        services.AddScoped<ProjectRepository>();
        services.AddScoped<IProjectRepository>(provider => provider.GetRequiredService<ProjectRepository>());
        services.AddScoped<IProjectQueries>(provider => provider.GetRequiredService<ProjectRepository>());
        
        services.AddScoped<TagRepository>();
        services.AddScoped<ITagRepository>(provider => provider.GetRequiredService<TagRepository>());
        services.AddScoped<ITagQueries>(provider => provider.GetRequiredService<TagRepository>());
        
        services.AddScoped<TagProjectRepository>();
        services.AddScoped<ITagProjectRepository>(provider => provider.GetRequiredService<TagProjectRepository>());
        services.AddScoped<ITagProjectQueries>(provider => provider.GetRequiredService<TagProjectRepository>());
        
        services.AddScoped<CategoryRepository>();
        services.AddScoped<ICategoryRepository>(provider => provider.GetRequiredService<CategoryRepository>());
        services.AddScoped<ICategoryQueries>(provider => provider.GetRequiredService<CategoryRepository>());
        
        services.AddScoped<PriorityRepository>();
        services.AddScoped<IPriorityRepository>(provider => provider.GetRequiredService<PriorityRepository>());
        services.AddScoped<IPriorityQueries>(provider => provider.GetRequiredService<PriorityRepository>());
        
        services.AddScoped<StatusRepository>();
        services.AddScoped<IStatusRepository>(provider => provider.GetRequiredService<StatusRepository>());
        services.AddScoped<IStatusQueries>(provider => provider.GetRequiredService<StatusRepository>());
        
        services.AddScoped<ProjectUserRepository>();
        services.AddScoped<IProjectUserRepository>(provider => provider.GetRequiredService<ProjectUserRepository>());
        services.AddScoped<IProjectUserQueries>(provider => provider.GetRequiredService<ProjectUserRepository>());
        
        services.AddScoped<RoleRepository>();
        services.AddScoped<IRoleRepository>(provider => provider.GetRequiredService<RoleRepository>());
        services.AddScoped<IRoleQueries>(provider => provider.GetRequiredService<RoleRepository>());
        
        services.AddScoped<TaskRepository>();
        services.AddScoped<ITaskRepository>(provider => provider.GetRequiredService<TaskRepository>());
        services.AddScoped<ITaskQueries>(provider => provider.GetRequiredService<TaskRepository>());
    }
}
