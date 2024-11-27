using Domain.Projects;
using Domain.Users;

namespace Domain.Comments;

public class Comment
{
    public CommentId Id { get; }

    public string Content { get; private set; }

    public UserId UserId { get; private set; }
    public User User { get; }

    public ProjectId ProjectId { get; private set; }
    public Project Project { get; }

    public DateTime PostedAt { get; private set; }

    private Comment(
        CommentId id,
        string content,
        UserId userId,
        ProjectId projectId,
        DateTime postedAt)
    {
        Id = id;
        Content = content;
        UserId = userId;
        ProjectId = projectId;
        PostedAt = postedAt;
    }

    public static Comment PostComment(
        CommentId commentId,
        string content,
        UserId user,
        ProjectId projectId,
        DateTime postedAt)
    => new(commentId, content, user, projectId, postedAt);
}