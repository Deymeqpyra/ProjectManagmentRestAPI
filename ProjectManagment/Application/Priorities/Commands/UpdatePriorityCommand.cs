using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Priorities.Exceptions;
using Domain.Priorities;
using MediatR;

namespace Application.Priorities.Commands;

public class UpdatePriorityCommand : IRequest<Result<ProjectPriority, PriorityException>>
{
    public required Guid PriorityId { get; init; }
    public required string UpdateName { get; init; }
}

public class UpdatePriorityCommandHandler(IPriorityRepository repository)
    : IRequestHandler<UpdatePriorityCommand, Result<ProjectPriority, PriorityException>>
{
    public async Task<Result<ProjectPriority, PriorityException>> Handle(UpdatePriorityCommand request,
        CancellationToken cancellationToken)
    {
        var exsistingPriority = await repository.GetById(new ProjectPriorityId(request.PriorityId), cancellationToken);

        return await exsistingPriority.Match(
            async p => await UpdateEntity(p, request.UpdateName, cancellationToken),
            () => Task.FromResult<Result<ProjectPriority, PriorityException>>(
                new PriorityNotFoundException(new ProjectPriorityId(request.PriorityId))));
    }

    private async Task<Result<ProjectPriority, PriorityException>> UpdateEntity(
        ProjectPriority priority,
        string updateName,
        CancellationToken cancellationToken)
    {
        try
        {
            priority.UpdateDetaile(updateName);
            return await repository.Update(priority, cancellationToken);
        }
        catch (Exception e)
        {
            return new PriorityUnknownException(priority.Id, e);
        }
    }
}