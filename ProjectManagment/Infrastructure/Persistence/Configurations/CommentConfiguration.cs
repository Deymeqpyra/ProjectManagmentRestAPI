using Domain.Comments;
using Domain.Projects;
using Domain.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x=>x.Value, x=> new CommentId(x));
        
        builder.Property(x => x.Content).HasMaxLength(500).IsRequired();

        builder.Property(x => x.PostedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc',now())");

        builder.Property(x => x.ProjectId)
            .HasConversion(x => x.value, x => new ProjectId(x));

        builder.Property(x => x.UserId)
            .HasConversion(x => x.value, x => new UserId(x));
        
        builder.HasOne(x => x.Project)
            .WithMany(e=>e.Comments)
            .HasForeignKey(x => x.ProjectId)
            .HasConstraintName("fk_project_comments")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_user_comments")
            .OnDelete(DeleteBehavior.Restrict);
    }
}