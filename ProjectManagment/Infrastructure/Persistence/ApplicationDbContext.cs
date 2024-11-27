using System.Reflection;
using Domain.Categories;
using Domain.Priorities;
using Domain.Projects;
using Domain.ProjectUsers;
using Domain.Roles;
using Domain.Statuses;
using Domain.Tags;
using Domain.TagsProjects;
using Domain.Tasks;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TagsProject> TagsProjects { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProjectPriority> ProjectPriorities { get; set; }
    public DbSet<ProjectStatus> ProjectStatuses { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectUser> ProjectUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        DataSeed.DataSeeder(builder);
        base.OnModelCreating(builder);
    }

    
}