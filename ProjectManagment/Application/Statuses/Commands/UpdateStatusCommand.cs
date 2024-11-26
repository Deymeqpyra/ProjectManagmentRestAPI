using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Statuses.Exceptions;
using Domain.Statuses;
using MediatR;

namespace Application.Statuses.Commands;

public class UpdateStatusCommand : IRequest<Result<ProjectStatus, StatusException>>
{
    public required Guid StatusId { get; init; }
    public required string StatusName { get; init; }
}

public class UpdateStatusCommandHandler(IStatusRepository repository)
    : IRequestHandler<UpdateStatusCommand, Result<ProjectStatus, StatusException>>
{
    public async Task<Result<ProjectStatus, StatusException>> Handle(UpdateStatusCommand request,
        CancellationToken cancellationToken)
    {
        var exsitingStatus = await repository.GetById(new ProjectStatusId(request.StatusId), cancellationToken);

        return await exsitingStatus.Match(
            async s => await UpdateEntity(s, request.StatusName, cancellationToken),
            () => Task.FromResult<Result<ProjectStatus, StatusException>>(
                new StatusNotFoundException(new ProjectStatusId(request.StatusId))));
    }

    private async Task<Result<ProjectStatus, StatusException>> UpdateEntity(
        ProjectStatus status,
        string name,
        CancellationToken cancellationToken)
    {
        try
        {
            status.UpdateDetails(name);

            return await repository.Update(status, cancellationToken);
        }
        catch (Exception e)
        {
            return new StatusUnknownException(ProjectStatusId.Empty(), e);
        }
    }
}