using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x=>x.value, x=>new UserId(x));
        
        builder.Property(x => x.Name).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.Email).IsRequired().HasColumnType("varchar(255)");
        
        builder.HasOne(x=>x.Role)
            .WithMany()
            .HasForeignKey(x=>x.RoleId)
            .HasConstraintName("fk_user_role_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x=>x.ProjectTask)
            .WithMany()
            .HasForeignKey(x=>x.ProjectTaskId)
            .HasConstraintName("fk_user_project_task_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}