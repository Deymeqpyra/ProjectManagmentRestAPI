using Domain.Tags;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ITagRepository
{
    Task<Option<Tag>> GetById(TagId tagId, CancellationToken cancellationToken);

    Task<Option<Tag>> GetByName(string name, CancellationToken cancellationToken);
    Task<Tag> Create(Tag tag, CancellationToken cancellationToken);
    Task<Tag> Update(Tag tag, CancellationToken cancellationToken);
    Task<Tag> Delete(Tag tag, CancellationToken cancellationToken);
}