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

namespace Api.Test.Integration.RoleController;

public class DeleteTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly Role _roleToDelete;
    private readonly User _adminUser;

    public DeleteTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _roleToDelete = RoleData.ExtraRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
    }

    [Fact]
    public async Task ShouldDeleteRole_WhenAuthorized()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        // Act
        var response = await Client.DeleteAsync($"role/DeleteRole/{_roleToDelete.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var roleFromDb = await Context.Roles.FirstOrDefaultAsync(x => x.Id == _roleToDelete.Id);
        roleFromDb.Should().BeNull();
    }

    [Fact]
    public async Task ShouldFailToDeleteRole_WhenUnauthorized()
    {
        // Act
        var response = await Client.DeleteAsync($"role/DeleteRole/{_roleToDelete.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToDeleteRole_WhenRoleDoesNotExist()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var nonExistentRoleId = RoleId.New();

        // Act
        var response = await Client.DeleteAsync($"role/DeleteRole/{nonExistentRoleId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain($"Role with id: {nonExistentRoleId} not found");
    }

    public async Task InitializeAsync()
    {
        await Context.Roles.AddRangeAsync(_roleToDelete);
        await Context.Roles.AddRangeAsync(_adminRole);
        await Context.Users.AddRangeAsync(_adminUser);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Users.RemoveRange(Context.Users);
        Context.Roles.RemoveRange(Context.Roles);
        await SaveChangesAsync();
    }
}
