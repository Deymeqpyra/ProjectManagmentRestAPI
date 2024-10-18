using Domain.Categories;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.HasKey(x => x.ProjectTaskId);
        builder.Property(x => x.ProjectTaskId)
            .HasConversion(x => x.value, x => new ProjectTaskId(x));
        
        
        
        
        builder.Property(x => x.CategoryId)
            .HasConversion(x => x.Value, x => new CategoryId(x));
        
        builder.HasOne(x=>x.Category)
            .WithMany()
            .HasForeignKey(x=>x.CategoryId)
            .HasConstraintName("fk_project_task_category_id")
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}