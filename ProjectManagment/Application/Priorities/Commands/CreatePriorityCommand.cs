using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Priorities.Exceptions;
using Domain.Priorities;
using MediatR;

namespace Application.Priorities.Commands;

public class CreatePriorityCommand : IRequest<Result<ProjectPriority, PriorityException>>
{
    public required string Name { get; init; }
}
public class CreatePriorityCommandHandler(IPriorityRepository repository) : IRequestHandler<CreatePriorityCommand, Result<ProjectPriority, PriorityException>>
{
    public async Task<Result<ProjectPriority, PriorityException>> Handle(CreatePriorityCommand request, CancellationToken cancellationToken)
    {
        var exsitingPriority = await repository.GetByName(request.Name, cancellationToken);

        return await exsitingPriority.Match(
            p => Task.FromResult<Result<ProjectPriority, PriorityException>>(new PriorityAlreadyExistsException(p.Id)),
            async () => await CreateEntity(request.Name, cancellationToken));
    }

    private async Task<Result<ProjectPriority, PriorityException>> CreateEntity(
        string priorityName, 
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = ProjectPriority.New(ProjectPriorityId.New() ,priorityName);
            
            return await repository.Create(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new PriorityUnknownException(ProjectPriorityId.Empty(), e);
        }
    }
}