using Domain.Priorities;

namespace Api.Dtos.PriorityDto;

public record PriorityDto(
    Guid? PriorityId,
    string Name)
{
    public static PriorityDto FromDomainModel(ProjectPriority priority)
        => new(
            PriorityId: priority.Id.value,
            Name: priority.Name);
}