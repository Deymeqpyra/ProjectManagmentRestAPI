using Api.Dtos.TasksDto;
using Domain.Users;

namespace Api.Dtos.UsersDto;

public record UserShortInfoForProjectDto(
    string UserName,
    string Email,
    UserTaskDto? UserTask)
{
    public static UserShortInfoForProjectDto FromUser(User user)
        => new(
            UserName: user.UserName,
            Email: user.Email,
            UserTask: user?.ProjectTask == null ? null : UserTaskDto.FromTask(user.ProjectTask));
}