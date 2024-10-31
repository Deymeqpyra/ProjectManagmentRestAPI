using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Statuses.Exceptions;
using Domain.Statuses;
using MediatR;

namespace Application.Statuses.Commands;

public class CreateStatusCommand : IRequest<Result<ProjectStatus, StatusException>>
{
    public required string Name { get; init;}
}
public class CreateStatusCommandHandler(IStatusRepository repository) : IRequestHandler<CreateStatusCommand, Result<ProjectStatus, StatusException>>
{
    public async Task<Result<ProjectStatus, StatusException>> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
    {
        var exisitingStatus = await repository.GetByName(request.Name, cancellationToken);

        return await exisitingStatus.Match(
            s => Task.FromResult<Result<ProjectStatus, StatusException>>(new StatusAlreadyExistsException(s.Id)),
            async () => await CreateEntity(request.Name, cancellationToken));
    }

    private async Task<Result<ProjectStatus, StatusException>> CreateEntity(string name, CancellationToken cancellationToken)
    {
        try
        {
            var entity = ProjectStatus.New(ProjectStatusId.New(), name);
            
            return await repository.Create(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new StatusUnknownException(ProjectStatusId.Empty(), e);
        }
    }
}