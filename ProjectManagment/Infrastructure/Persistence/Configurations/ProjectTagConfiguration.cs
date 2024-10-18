using Domain.Projects;
using Domain.Tags;
using Domain.TagsProjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProjectTagConfiguration : IEntityTypeConfiguration<TagsProject>
{
    public void Configure(EntityTypeBuilder<TagsProject> builder)
    {
        builder.HasKey(x=>x.Id);
        builder.Property(x=>x.TagId)
            .HasConversion(x=>x.value, x =>new TagId(x));
        builder.Property(x=>x.ProjectId)
            .HasConversion(x=>x.value, x =>new ProjectId(x));
        
        
        builder.HasOne(p => p.Project)
            .WithMany(tp => tp.TagsProjects)
            .HasForeignKey(x => x.ProjectId)
            .HasConstraintName("fk_tagprojects_project_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(t=>t.Tag)
            .WithMany(t=>t.TagsProjects)
            .HasForeignKey(t => t.TagId)
            .HasConstraintName("fk_tagprojects_tag_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}