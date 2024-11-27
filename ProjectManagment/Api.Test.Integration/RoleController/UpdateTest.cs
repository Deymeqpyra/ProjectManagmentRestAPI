using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Api.Dtos.RolesDto;
using Domain.Roles;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.RoleController;

public class UpdateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private readonly Role _existingRole;

    public UpdateTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
        _existingRole = RoleData.ExtraRole();
    }

    [Fact]
    public async Task ShouldUpdateRole()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        const string updatedName = "Updated Role Name";
        var request = new CreateRoleDto(Name: updatedName);

        // Act
        var response = await Client.PutAsJsonAsync($"role/UpdateRole/{_existingRole.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var roleFromDatabase = await Context.Roles.FirstOrDefaultAsync(x => x.Id == _existingRole.Id);
        roleFromDatabase.Should().NotBeNull();
        roleFromDatabase!.Name.Should().Be(updatedName);
    }

    [Fact]
    public async Task ShouldFailToUpdateRole_WhenUnauthorized()
    {
        // Arrange
        const string updatedName = "Unauthorized Update";
        var request = new CreateRoleDto(Name: updatedName);

        // Act
        var response = await Client.PutAsJsonAsync($"role/UpdateRole/{_existingRole.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToUpdateRole_WhenNameIsEmpty()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var request = new CreateRoleDto(Name: string.Empty);

        // Act
        var response = await Client.PutAsJsonAsync($"role/UpdateRole/{_existingRole.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Name is required");
    }

    [Fact]
    public async Task ShouldFailToUpdateRole_WhenNameIsTooLong()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var longName = new string('a', 101);
        var request = new CreateRoleDto(Name: longName);

        // Act
        var response = await Client.PutAsJsonAsync($"role/UpdateRole/{_existingRole.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Name is too long");
    }

    [Fact]
    public async Task ShouldFailToUpdateRole_WhenIdDoesNotExist()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var nonExistentRoleId = RoleId.New();
        var request = new CreateRoleDto(Name: "Non-existent Role");

        // Act
        var response = await Client.PutAsJsonAsync($"role/UpdateRole/{nonExistentRoleId}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain($"Role with id: {nonExistentRoleId} not found");
    }

    public async Task InitializeAsync()
    {
        await Context.Roles.AddRangeAsync(_adminRole, _existingRole);
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