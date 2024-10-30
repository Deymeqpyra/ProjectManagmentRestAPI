using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Priorities.Exceptions;
using Domain.Priorities;
using MediatR;

namespace Application.Priorities.Commands;

public class DeletePriorityCommand : IRequest<Result<ProjectPriority, PriorityException>>
{
    public required Guid PriorityId { get; init; }
}

public class DeletePriorityCommandHandler(IPriorityRepository repository)
    : IRequestHandler<DeletePriorityCommand, Result<ProjectPriority, PriorityException>>
{
    public async Task<Result<ProjectPriority, PriorityException>> Handle(DeletePriorityCommand request,
        CancellationToken cancellationToken)
    {
        var exsistingPriority = await repository.GetById(new ProjectPriorityId(request.PriorityId), cancellationToken);

        return await exsistingPriority.Match(
            async p => await DeleteEntity(p, cancellationToken),
            () => Task.FromResult<Result<ProjectPriority, PriorityException>>(
                new PriorityNotFoundException(new ProjectPriorityId(request.PriorityId))));
    }

    private async Task<Result<ProjectPriority, PriorityException>> DeleteEntity(
        ProjectPriority priority,
        CancellationToken cancellationToken)
    {
        try
        {
            return await repository.Delete(priority, cancellationToken);
        }
        catch (Exception e)
        {
            return new PriorityUnknownException(priority.Id, e);
        }
    }
}