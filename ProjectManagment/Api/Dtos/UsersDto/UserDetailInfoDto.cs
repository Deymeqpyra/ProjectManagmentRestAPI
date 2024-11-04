using Api.Dtos.RolesDto;
using Api.Dtos.TasksDto;
using Domain.Roles;
using Domain.Tasks;
using Domain.Users;

namespace Api.Dtos.UsersDto;

public record UserDetailInfoDto(
    Guid? userId,
    string name,
    string password,
    string email,
    Guid roleId,
    RoleDto? role,
    Guid? TaskId, 
    UserTaskDto? UserTask
    )
{
    public static UserDetailInfoDto FromUser(User user)
        => new(
            userId: user.Id.value,
            name: user.UserName,
            password: user.Password,
            email: user.Email,
            roleId: user.RoleId.value,
            role: user.Role == null ? null : RoleDto.FromDomainModel(user.Role),
            TaskId: user.ProjectTaskId?.value,
            UserTask: user.ProjectTask == null ? null : UserTaskDto.FromTask(user.ProjectTask)
        );
}