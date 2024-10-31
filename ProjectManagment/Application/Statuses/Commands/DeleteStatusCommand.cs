using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Statuses.Exceptions;
using Domain.Statuses;
using MediatR;

namespace Application.Statuses.Commands;

public class DeleteStatusCommand : IRequest<Result<ProjectStatus, StatusException>>
{
    public required Guid StatusId { get; init; }
}

public class DeleteStatusCommandHandler(IStatusRepository repository)
    : IRequestHandler<DeleteStatusCommand, Result<ProjectStatus, StatusException>>
{
    public async Task<Result<ProjectStatus, StatusException>> Handle(DeleteStatusCommand request,
        CancellationToken cancellationToken)
    {
        var existingStatus = await repository.GetById(new ProjectStatusId(request.StatusId), cancellationToken);

        return await existingStatus.Match(
            async s => await DeleteEntity(s, cancellationToken),
            () => Task.FromResult<Result<ProjectStatus, StatusException>>(
                new StatusNotFoundException(new ProjectStatusId(request.StatusId))));
    }

    private async Task<Result<ProjectStatus, StatusException>> DeleteEntity(
        ProjectStatus status,
        CancellationToken cancellationToken)
    {
        try
        {
            return await repository.Delete(status, cancellationToken);
        }
        catch (Exception e)
        {
            return new StatusUnknownException(ProjectStatusId.Empty(), e);
        }
    }
}