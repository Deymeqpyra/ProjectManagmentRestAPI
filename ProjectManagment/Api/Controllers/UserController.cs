using Api.Dtos.UsersDto;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Users.Commands;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("users")]
[ApiController]
public class UserController(ISender sender, IUserQueries userQueries) : ControllerBase
{
    [Authorize(Roles = "Admin, User")]
    [HttpGet("getall")]
    public async Task<ActionResult<IReadOnlyList<UserDetailInfoDto>>> GetAllUsers(CancellationToken cancellationToken)
    {
        var entites = await userQueries.GetAll(cancellationToken);

        return entites.Select(UserDetailInfoDto.FromUser).ToList();
    }
    
    [Authorize(Roles = "Admin, User")]
    [HttpGet("getbyid/{userId:guid}")]
    public async Task<ActionResult<UserDetailInfoDto>> GetUserById([FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var entity = await userQueries.GetByIdWithRoles(new UserId(userId), cancellationToken);

        return entity.Match<ActionResult<UserDetailInfoDto>>(
            u => UserDetailInfoDto.FromUser(u),
            () => NotFound());
    }

    [HttpPost("register")]
    public async Task<ActionResult<CreateUserDto>> Create([FromBody] CreateUserDto createUserDto,
        CancellationToken cancellationToken)
    {
        var input = new RegisterUserCommand
        {
            Email = createUserDto.Email,
            Password = createUserDto.Password,
            Username = createUserDto.UserName
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CreateUserDto>>
        (u => CreateUserDto.FromUser(u),
            e => e.ToObjectResult());
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult<string>> LoginUser([FromBody] LoginUserDto loginUserDto,
        CancellationToken cancellationToken)
    {
        var input = new LoginUserCommand
        {
            Email = loginUserDto.email,
            PassWord = loginUserDto.password
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<string>>
        (token => token,
            e => e.ToObjectResult());
    }

    [HttpPut("updateUserName/{userId:guid}")]
    public async Task<ActionResult<UserDetailInfoDto>> UpdateName([FromRoute] Guid userId, [FromBody] string name,
        CancellationToken cancellationToken)
    {
        var input = new UpdateUserNameCommand
        {
            UserId = userId,
            UserName = name
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UserDetailInfoDto>>(
            u => UserDetailInfoDto.FromUser(u),
            e => e.ToObjectResult());
    }

    [HttpPut("updatePassword/{userId:guid}")]
    public async Task<ActionResult<UserDetailInfoDto>> UpdatePassword(
        [FromRoute] Guid userId,
        [FromBody] string password,
        CancellationToken cancellationToken)
    {
        var input = new UpdateUserPasswordCommand
        {
            UserId = userId,
            Password = password
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UserDetailInfoDto>>(
            u => UserDetailInfoDto.FromUser(u),
            e => e.ToObjectResult());
    }

    [HttpPut("updateEmail/{userId:guid}")]
    public async Task<ActionResult<UserDetailInfoDto>> UpdateEmail(
        [FromRoute] Guid userId,
        [FromBody] string email,
        CancellationToken cancellationToken)
    {
        var input = new UpdateUserEmailCommand
        {
            UserId = userId,
            Email = email
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UserDetailInfoDto>>(
            u => UserDetailInfoDto.FromUser(u),
            e => e.ToObjectResult());
    }

    [HttpPut("assignUser/{userId:guid}/toTask/{taskId:guid}")]
    public async Task<ActionResult<UserDetailInfoDto>> AssignTask([FromRoute] Guid userId, [FromRoute] Guid taskId,
        CancellationToken cancellationToken)
    {
        var input = new AssignUserToTaskCommand
        {
            UserId = userId,
            TaskId = taskId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UserDetailInfoDto>>(
            u => UserDetailInfoDto.FromUser(u),
            e => e.ToObjectResult());
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{userId:guid}")]
    public async Task<ActionResult<UserDetailInfoDto>> Delete([FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var input = new DeleteUserCommand
        {
            UserId = userId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UserDetailInfoDto>>(
            u => UserDetailInfoDto.FromUser(u),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("giveUser/{userId:guid}/role/{roleId:guid}")]
    public async Task<ActionResult<UserDetailInfoDto>> GiveUserToRole([FromRoute] Guid userId, [FromRoute] Guid roleId,
        CancellationToken cancellationToken)
    {
        var input = new GiveNewRoleToUserCommand
        {
            UserId = userId,
            RoleId = roleId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UserDetailInfoDto>>(
            u => UserDetailInfoDto.FromUser(u),
            e => e.ToObjectResult());
    }
}