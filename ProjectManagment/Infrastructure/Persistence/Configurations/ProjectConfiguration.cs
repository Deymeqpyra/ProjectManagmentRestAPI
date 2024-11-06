using System.ComponentModel;
using Domain.Priorities;
using Domain.Projects;
using Domain.Statuses;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.ProjectId);
        builder.Property(x=>x.ProjectId).HasConversion(x=>x.value, x=>new ProjectId(x));
        
        builder.Property(x=>x.Title).IsRequired().HasColumnType("varchar(100)");
        builder.Property(x=>x.Description).IsRequired().HasColumnType("varchar(255)");

        builder.Property(x => x.LastUpdate)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc',now())");

        builder.Property(x => x.ProjectStatusId)
            .HasConversion(x => x.value, x => new ProjectStatusId(x));

        builder.Property(x => x.ProjectPriorityId)
            .HasConversion(x => x.value, x => new ProjectPriorityId(x));
        
        builder.HasOne(x => x.ProjectStatus)
            .WithMany()
            .HasForeignKey(x => x.ProjectStatusId)
            .HasConstraintName("fk_project_status_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.ProjectPriority)
            .WithMany()
            .HasForeignKey(x => x.ProjectPriorityId)
            .HasConstraintName("fk_project_priority_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_project_user_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x=>x.Comments);
    }
}