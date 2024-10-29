using Application.Categories.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Tags.Exceptions;
using Domain.Tags;
using MediatR;

namespace Application.Tags.Commands;

public class UpdateTagCommand : IRequest<Result<Tag, TagException>>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}

public class UpdateTagCommandHandler(ITagRepository tagRepository)
    : IRequestHandler<UpdateTagCommand, Result<Tag, TagException>>
{
    public async Task<Result<Tag, TagException>> Handle(
        UpdateTagCommand request,
        CancellationToken cancellationToken)
    {
        var tagId = new TagId(request.Id);
        var existingTag = await tagRepository.GetById(tagId, cancellationToken);

        return await existingTag.Match(
            async c => await UpdateEntity(c, request.Name, cancellationToken),
            () => Task.FromResult<Result<Tag, TagException>>(new TagNotFoundException(tagId)));
    }

    private async Task<Result<Tag, TagException>> UpdateEntity(
        Tag tag,
        string name,
        CancellationToken cancellationToken)
    {
        try
        {
            tag.UpdateDetails(name);
            
            return await tagRepository.Update(tag, cancellationToken);
        }
        catch (Exception e)
        {
            return new TagUnknownException(tag.Id, e);
        }
    }
}