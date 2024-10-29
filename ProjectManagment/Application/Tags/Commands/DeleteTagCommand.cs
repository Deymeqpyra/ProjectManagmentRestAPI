using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Tags.Exceptions;
using Domain.Tags;
using MediatR;

namespace Application.Tags.Commands;

public class DeleteTagCommand : IRequest<Result<Tag, TagException>>
{
    public required Guid TagId { get; init; }
}

public class DeleteTagCommandHandler(ITagRepository repository)
    : IRequestHandler<DeleteTagCommand, Result<Tag, TagException>>
{
    public async Task<Result<Tag, TagException>> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tagId = new TagId(request.TagId);
        
        var exisitingTag = await repository.GetById(tagId, cancellationToken);

        return await exisitingTag.Match(
            async t => await DeleteEnitity(t, cancellationToken),
            () => Task.FromResult<Result<Tag, TagException>>(new TagNotFoundException(tagId)));
    }

    private async Task<Result<Tag, TagException>> DeleteEnitity(
        Tag tag,
        CancellationToken cancellationToken)
    {
        try
        {
            return await repository.Delete(tag, cancellationToken);
        }
        catch (Exception e)
        {
            return new TagUnknownException(TagId.Empty(), e);
        }
    }
}