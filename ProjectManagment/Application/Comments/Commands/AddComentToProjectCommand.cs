using Application.Comments.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Projects.Exceptions;
using Domain.Comments;
using Domain.Projects;
using Domain.Users;
using MediatR;
using CommentUnknownException = Application.Comments.Exceptions.CommentUnknownException;
using ProjectNotFound = Application.Comments.Exceptions.ProjectNotFound;

namespace Application.Comments.Commands;

public class AddComentToProjectCommand : IRequest<Result<Comment, CommentException>>
{
    public required Guid ProjectId { get; init; }
    public required Guid UserId { get; init; }
    public required string CommentMessage { get; init; }
}

public class AddCommentToProjectCommandHandler(IProjectRepository repository,
    IUserRepository userRepository, ICommentRepository commentRepository)
    : IRequestHandler<AddComentToProjectCommand, Result<Comment, CommentException>>
{
    public async Task<Result<Comment, CommentException>> Handle(AddComentToProjectCommand request,
        CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        var userId = new UserId(request.UserId);

        var existingProject = await repository.GetById(projectId, cancellationToken);
        var exsistingUser = await userRepository.GetById(userId, cancellationToken);

        return await exsistingUser.Match(
            async u =>
            {
                return await existingProject.Match(
                    async p => { return await CreateComment(CommentId.New(),p.ProjectId, u.Id,request.CommentMessage, cancellationToken); },
                    () => Task.FromResult<Result<Comment, CommentException>>(new ProjectNotFound(projectId, CommentId.Empty())));
            },
            () => Task.FromResult<Result<Comment, CommentException>>(new UserNotFound(userId, CommentId.Empty()))
        );
    }

    private async Task<Result<Comment, CommentException>> CreateComment(CommentId commentId,ProjectId projectId, UserId userId,
        string comment, CancellationToken cancellationToken)
    {
        try
        {
            var commentEntity = Comment.PostComment(commentId, comment, userId, projectId, DateTime.UtcNow);
            
            return await commentRepository.Create(commentEntity, cancellationToken); 
        }
        catch (Exception e)
        {
            return new CommentUnknownException(commentId, e);
        }
    }
}