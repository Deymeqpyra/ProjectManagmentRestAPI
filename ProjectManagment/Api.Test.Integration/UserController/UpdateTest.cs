using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Domain.Roles;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.UserController;

public class UpdateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly Role _userRole;
    private readonly User _mainUser;

    public UpdateTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _userRole = RoleData.UserRole();
        _mainUser = UserData.ExtraUser(_userRole.Id);
    }
    [Fact]
    public async Task ShouldUpdateUserEmail_Success()
    {
        
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_mainUser.Email, UserData.passwordUser);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var newEmail = "updatedemail@test.com";

        // Act
        var response = await Client.PutAsJsonAsync($"users/updateEmail/{_mainUser.Id.value}", newEmail);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var updatedUser = await Context.Users.FirstOrDefaultAsync(u => u.Id == _mainUser.Id);
        updatedUser.Should().NotBeNull();
        updatedUser!.Email.Should().Be(newEmail);
    }

    [Fact]
    public async Task ShouldFailToUpdateUserEmail_WhenUnauthorized()
    {
        // Arrange
        var newEmail = "newunauthorizedemail@test.com";

        // Act
        var response = await Client.PutAsJsonAsync($"users/updateEmail/{_mainUser.Id.value}", newEmail);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
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