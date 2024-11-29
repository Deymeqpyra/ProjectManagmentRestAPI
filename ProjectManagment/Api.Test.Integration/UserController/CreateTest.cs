using System.Net;
using System.Net.Http.Json;
using Api.Dtos.UsersDto;
using Domain.Roles;
using Domain.Tags;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.UserController;

public class CreateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly Role _userRole;
    private readonly User _mainUser;

    public CreateTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _userRole = RoleData.UserRole();
        _mainUser = UserData.ExtraUser(_userRole.Id);
    }
    [Fact]
    public async Task ShouldCreateUser_Success()
    {
        // Arrange
        var request = new CreateUserDto(Email:"newuser@test.com", Password:"123456", UserName:"UserTest");

        // Act
        var response = await Client.PostAsJsonAsync("users/register", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var createdUser = await response.Content.ReadFromJsonAsync<CreateUserDto>();
        createdUser.Should().NotBeNull();
        createdUser!.Email.Should().Be(request.Email);
        createdUser.UserName.Should().Be(request.UserName);

        var userFromDb = await Context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        userFromDb.Should().NotBeNull();
        userFromDb!.UserName.Should().Be(request.UserName);
    }

    [Fact]
    public async Task ShouldFailToCreateUser_WithDuplicateEmail()
    {
        // Arrange
        var request = new CreateUserDto(Email:_mainUser.Email, Password:"123456", UserName:"UserTest");

        // Act
        var response = await Client.PostAsJsonAsync("users/register", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    public async Task InitializeAsync()
    {
        await Context.Roles.AddRangeAsync(_adminRole,_userRole);
        await Context.Users.AddRangeAsync(_mainUser);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.RemoveRange(Context.Users);
        Context.RemoveRange(Context.Roles);
        await Context.SaveChangesAsync();
    }
}