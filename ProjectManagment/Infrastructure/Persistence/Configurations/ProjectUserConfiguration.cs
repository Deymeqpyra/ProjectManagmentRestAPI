using Domain.Projects;
using Domain.ProjectUsers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProjectUserConfiguration : IEntityTypeConfiguration<ProjectUser>
{
    public void Configure(EntityTypeBuilder<ProjectUser> builder)
    {
        builder.HasKey(x => x.ProjectUserId);

        builder.Property(x => x.ProjectId)
            .HasConversion(x => x.value, x => new ProjectId(x));
        builder.Property(x => x.UserId)
            .HasConversion(x => x.value, x => new UserId(x));
        
        builder.HasOne(x => x.Project)
            .WithMany(pu=>pu.ProjectUsers)
            .HasForeignKey(x => x.ProjectId)
            .HasConstraintName("fk_projectuser_project")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.User)
            .WithMany(pu => pu.ProjectUsers)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_projectuser_user")
            .OnDelete(DeleteBehavior.Restrict);
    }
}