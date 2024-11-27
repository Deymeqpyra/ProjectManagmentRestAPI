using Domain.Categories;
using Domain.Priorities;
using Domain.Roles;
using Domain.Statuses;
using Domain.Tags;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class DataSeed
{
    public static void DataSeeder(ModelBuilder builder)
    {
        builder.Entity<Category>()
            .HasData(
                Category.New(CategoryId.New(), "Development"),
                Category.New(CategoryId.New(), "Documentary"),
                Category.New(CategoryId.New(), "Learning")
            );
        builder.Entity<ProjectStatus>()
            .HasData(
                ProjectStatus.New(ProjectStatusId.New(), "Started"),
                ProjectStatus.New(ProjectStatusId.New(), "In progress"),
                ProjectStatus.New(ProjectStatusId.New(), "Finished")
            );
        builder.Entity<ProjectPriority>()
            .HasData(
                ProjectPriority.New(ProjectPriorityId.New(), "Very Low"),
                ProjectPriority.New(ProjectPriorityId.New(), "Medium"),
                ProjectPriority.New(ProjectPriorityId.New(), "High"),
                ProjectPriority.New(ProjectPriorityId.New(), "Very High")
            );
        
        RoleId userRole = RoleId.New();
        RoleId adminRole = RoleId.New();
        builder.Entity<Role>()
            .HasData(
                Role.New(userRole, "User"),
                Role.New(adminRole,  "Admin")
            );
        
        builder.Entity<User>()
            .HasData(
                User.RegisterNewUser(UserId.New(),  "user", "userPass", "user@gmail.com",  userRole),
                User.RegisterNewUser(UserId.New(),  "admin", "adminPass", "admin@gmail.com",  adminRole)
            );

        builder.Entity<Tag>().HasData(
            Tag.New(TagId.New(), "school"),
            Tag.New(TagId.New(), "dotnet"),
            Tag.New(TagId.New(), "work"),
            Tag.New(TagId.New(), "petproject"),
            Tag.New(TagId.New(), "coding"),
            Tag.New(TagId.New(), "python"),
            Tag.New(TagId.New(), "university"),
            Tag.New(TagId.New(), "database"),
            Tag.New(TagId.New(), "datastructure")
        );
    }
}