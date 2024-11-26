using System.Net;
using System.Net.Http.Headers;
using Domain.Roles;
using Domain.Statuses;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.StatusController;

public class DeleteTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private readonly ProjectStatus _statusToDelete;

    public DeleteTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
        _statusToDelete = StatusData.MainStatus();
    }

    [Fact]
    public async Task ShouldDeleteStatus_WhenAuthorized()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        // Act
        var response = await Client.DeleteAsync($"status/Delete/{_statusToDelete.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var statusFromDb = await Context.ProjectStatuses.FirstOrDefaultAsync(x => x.Id == _statusToDelete.Id);
        statusFromDb.Should().BeNull();
    }

    [Fact]
    public async Task ShouldFailToDeleteStatus_WhenUnauthorized()
    {
        // Act
        var response = await Client.DeleteAsync($"status/Delete/{_statusToDelete.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToDeleteStatus_WhenStatusDoesNotExist()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var nonExistentStatusId = ProjectStatusId.New();

        // Act
        var response = await Client.DeleteAsync($"status/Delete/{nonExistentStatusId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public async Task InitializeAsync()
    {
        await Context.ProjectStatuses.AddRangeAsync(_statusToDelete);
        await Context.Roles.AddRangeAsync(_adminRole);
        await Context.Users.AddRangeAsync(_adminUser);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Users.RemoveRange(Context.Users);
        Context.Roles.RemoveRange(Context.Roles);
        Context.ProjectStatuses.RemoveRange(Context.ProjectStatuses);
        await SaveChangesAsync();
    }
}
