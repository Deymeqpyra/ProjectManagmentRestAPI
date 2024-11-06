using Domain.Categories;
using Domain.Projects;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.HasKey(x => x.TaskId);
        builder.Property(x => x.TaskId)
            .HasConversion(x => x.value, x => new ProjectTaskId(x));

        builder.Property(x => x.ProjectId)
            .HasConversion(x => x.value, x => new ProjectId(x));
        
        builder.Property(x => x.CategoryId)
            .HasConversion(x => x.Value, x => new CategoryId(x));
        
        builder.HasOne(x=>x.Category)
            .WithMany()
            .HasForeignKey(x=>x.CategoryId)
            .HasConstraintName("fk_project_task_category_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x=>x.Project)
            .WithMany()
            .HasForeignKey(x=>x.ProjectId)
            .HasConstraintName("fk_project_task_project_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_project_user_id")
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}