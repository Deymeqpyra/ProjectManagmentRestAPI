using System.Net.Http.Headers;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Tags.Exceptions;
using Domain.Tags;
using MediatR;

namespace Application.Tags.Commands;

public class CreateTagCommand : IRequest<Result<Tag, TagException>>
{
    public required string Name { get; init; }
}
public class CreateTagCommandHandler(ITagRepository tagRepository) 
    : IRequestHandler<CreateTagCommand, Result<Tag, TagException>>
{
    public async Task<Result<Tag, TagException>> Handle(
        CreateTagCommand request, 
        CancellationToken cancellationToken)
    {
        var existingTag = await tagRepository.GetByName(request.Name, cancellationToken);
        
        return await existingTag.Match(
            t=> Task.FromResult<Result<Tag, TagException>>( new TagAlreadyExistsException(t.Id)),
            async ()=> await CreateEntity(
                request.Name,
                cancellationToken));
    }

    private async Task<Result<Tag,TagException>> CreateEntity(string tagName, CancellationToken cancellationToken)
    {
        try
        {
            var entity = Tag.New(TagId.New(), tagName);
            
            return await tagRepository.Create(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new TagUnknownException(TagId.Empty(), e);
        }
        
    }
}