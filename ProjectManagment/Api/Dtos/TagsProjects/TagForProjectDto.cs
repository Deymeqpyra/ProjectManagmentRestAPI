using Api.Dtos.TagsDto;
using Domain.Tags;
using Domain.TagsProjects;

namespace Api.Dtos.TagsProjects;

public record TagForProjectDto(
    TagDto? Tag)
{
    public static TagForProjectDto FromTag(TagsProject tag)
        => new(
            Tag: tag.Tag == null ? null : TagDto.FromDomainModel(tag.Tag));
}