using Domain.Priorities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProjectPriorityConfiguration : IEntityTypeConfiguration<ProjectPriority>
{
    public void Configure(EntityTypeBuilder<ProjectPriority> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasConversion(x=>x.value, x=>new ProjectPriorityId(x));
        
        
        builder.Property(p => p.Name).IsRequired().HasColumnType("varchar(255)");
    }
}