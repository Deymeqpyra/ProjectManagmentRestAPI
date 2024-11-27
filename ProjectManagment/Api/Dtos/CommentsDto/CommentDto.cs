using Api.Dtos.UsersDto;
using Domain.Comments;

namespace Api.Dtos.CommentsDto;

public record CommentDto(
    Guid Id, 
    UserDtoForComment user,
    string content,
    DateTime dateCreated
    )
{
    public static CommentDto FromDomain(Comment domain)
        => new(Id: domain.Id.Value,
            user: domain.User == null ? null : UserDtoForComment.FromUser(domain.User),
            content: domain.Content,
            dateCreated: domain.PostedAt);
}