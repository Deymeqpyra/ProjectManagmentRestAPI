using System.Net.Http.Headers;
using Domain.Roles;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.UserController;

public class DeleteTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly Role _userRole;
    private readonly User _mainUser;
    private readonly User _mainAdminUser;

    public DeleteTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _userRole = RoleData.UserRole();
        _mainUser = UserData.ExtraUser(_userRole.Id);
        _mainAdminUser = UserData.AdminUser(_adminRole.Id);
    }
    [Fact]
    public async Task ShouldDeleteUser_Success()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_mainAdminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);


        // Act
        var response = await Client.DeleteAsync($"users/delete/{_mainUser.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var deletedUser = await Context.Users.FirstOrDefaultAsync(u => u.Id == _mainUser.Id);
        deletedUser.Should().BeNull();
    }

    [Fact]
    public async Task ShouldFailToDeleteUser_WhenUnauthorized()
    {
        // Act
        var response = await Client.DeleteAsync($"users/delete/{_mainUser.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToDeleteUser_WhenUserDoesNotExist()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_mainAdminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var nonExistentUserId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"users/delete/{nonExistentUserId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    public async Task InitializeAsync()
    {
        await Context.Roles.AddRangeAsync(_adminRole,_userRole);
        await Context.Users.AddRangeAsync(_mainUser, _mainAdminUser);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.RemoveRange(Context.Users);
        Context.RemoveRange(Context.Roles);
        await Context.SaveChangesAsync();
    }
}