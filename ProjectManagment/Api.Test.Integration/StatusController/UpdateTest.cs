using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Api.Dtos.StatusesDto;
using Domain.Roles;
using Domain.Statuses;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.StatusController;

public class UpdateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private readonly ProjectStatus _statusToUpdate;

    public UpdateTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
        _statusToUpdate = StatusData.MainStatus();
    }

    [Fact]
    public async Task ShouldUpdateStatus()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        const string updatedStatusName = "Updated Status";
        var request = new CreateStatusDto(Name: updatedStatusName);

        // Act
        var response = await Client.PutAsJsonAsync($"status/Update/{_statusToUpdate.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var statusFromDb = await Context.ProjectStatuses.FirstOrDefaultAsync(x => x.Id == _statusToUpdate.Id);
        statusFromDb.Should().NotBeNull();
        statusFromDb.Name.Should().Be(updatedStatusName);
    }

    [Fact]
    public async Task ShouldFailToUpdateStatus_WhenUnauthorized()
    {
        // Arrange
        var request = new CreateStatusDto(Name: "Updated Status");

        // Act
        var response = await Client.PutAsJsonAsync($"status/Update/{_statusToUpdate.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToUpdateStatus_WhenStatusDoesNotExist()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var nonExistentStatusId = ProjectStatusId.New();
        var request = new CreateStatusDto(Name: "Updated Status");

        // Act
        var response = await Client.PutAsJsonAsync($"status/Update/{nonExistentStatusId}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public async Task InitializeAsync()
    {
        await Context.ProjectStatuses.AddRangeAsync(_statusToUpdate);
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
