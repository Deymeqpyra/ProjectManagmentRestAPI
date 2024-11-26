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

public class CreateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private readonly Role _testRole;

    public CreateTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
        _testRole = RoleData.ExtraRole();
    }

    [Fact]
    public async Task ShouldCreateRole()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        const string roleName = "Test Role";
        var request = new CreateRoleDto(Name: roleName);

        // Act
        var response = await Client.PostAsJsonAsync("role/AddNewRole", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var roleFromResponse = await response.Content.ReadFromJsonAsync<RoleDto>();
        var roleId = new RoleId(roleFromResponse.roleId!.Value);

        var roleFromDataBase = await Context.Roles.FirstOrDefaultAsync(x => x.Id == roleId);

        roleFromDataBase.Should().NotBeNull();
        roleFromResponse!.RoleName.Should().Be(roleName);
    }

    [Fact]
    public async Task ShouldFailToCreateRole_WhenUnauthorized()
    {
        // Arrange
        const string roleName = "Unauthorized Role";
        var request = new CreateRoleDto(Name: roleName);

        // Act
        var response = await Client.PostAsJsonAsync("role/AddNewRole", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToCreateRole_WhenNameIsEmpty()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var request = new CreateRoleDto(Name: string.Empty);

        // Act
        var response = await Client.PostAsJsonAsync("role/AddNewRole", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Name is required");
    }

    [Fact]
    public async Task ShouldFailToCreateRole_WhenNameIsTooLong()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var longRoleName = new string('a', 101);
        var request = new CreateRoleDto(Name: longRoleName);

        // Act
        var response = await Client.PostAsJsonAsync("role/AddNewRole", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Name is too long");
    }

    [Fact]
    public async Task ShouldFailToCreateRole_WhenNameAlreadyExists()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var request = new CreateRoleDto(Name: _testRole.Name);

        // Act
        var response = await Client.PostAsJsonAsync("role/AddNewRole", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Role with id:");
    }

    public async Task InitializeAsync()
    {
        await Context.Roles.AddRangeAsync(_testRole);
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
