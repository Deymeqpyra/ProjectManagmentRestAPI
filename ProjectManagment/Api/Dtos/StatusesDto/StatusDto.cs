using Domain.Statuses;

namespace Api.Dtos.StatusesDto;

public record StatusDto(
    Guid? StatusId,
    string StatusName)
{
    public static StatusDto FromDomainModel(ProjectStatus status)
        =>new(StatusId: status.Id.value,
            StatusName: status.Name);
}