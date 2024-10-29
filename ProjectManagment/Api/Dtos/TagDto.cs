using Domain.Tags;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Api.Dtos;

public record TagDto(
    Guid? tagId,
    string name)
{
    public static TagDto FromDomainModel(Tag tag)
        => new(
            tagId: tag.Id.value,
            name: tag.Name);
}