using Api.Dtos.UsersDto;
using Domain.ProjectUsers;

namespace Api.Dtos.ProjectsUsersDto;

public record UserForProjectDto(UserShortInfoForProjectDto? UserShortInfo)
{
    public static UserForProjectDto FromUserShortInfo(ProjectUser user)
        => new(UserShortInfo: user.User == null ? null : UserShortInfoForProjectDto.FromUser(user.User));
}