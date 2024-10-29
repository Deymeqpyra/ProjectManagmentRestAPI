using Domain.Tags;

namespace Application.Tags.Exceptions;

public abstract class TagException(TagId tagId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public TagId TagId { get; } = tagId;
}

public class TagNotFoundException(TagId tagId)
    : TagException(tagId, $"Tag with {tagId} not found");

public class TagAlreadyExistsException(TagId tagId)
    : TagException(tagId, $"Tag already exists {tagId}");

public class TagUnknownException(TagId tagId, Exception innerException)
    : TagException(tagId, $"Unknown tag exception for id: {tagId}", innerException);