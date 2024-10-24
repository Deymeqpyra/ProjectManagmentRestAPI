using Domain.Tags;

namespace Application.Common.Interfaces.Repositories;

public interface ITagRepository
{
    Task<Tag> Create(Tag tag, CancellationToken cancellationToken);
    Task<Tag> Update(Tag tag, CancellationToken cancellationToken);
    Task<Tag> Delete(Tag tag, CancellationToken cancellationToken);
}