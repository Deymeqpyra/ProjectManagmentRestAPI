using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Api.Dtos.StatusesDto;
using Domain.Priorities;
using Domain.Roles;
using Domain.Statuses;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.StatusController;

public class CreateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private readonly ProjectStatus _mainStatus;

    public CreateTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _mainStatus = StatusData.MainStatus();
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
    }

    [Fact]
    public async Task ShouldCreateStatus()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        const string statusName = "Active";
        var request = new CreateStatusDto(Name: statusName);

        // Act
        var response = await Client.PostAsJsonAsync("status/Create", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var statusFromResponse = await response.Content.ReadFromJsonAsync<StatusDto>();
        var statusId = new ProjectStatusId(statusFromResponse.StatusId!.Value);

        var statusFromDatabase = await Context.ProjectStatuses.FirstOrDefaultAsync(x => x.Id == statusId);

        statusFromDatabase.Should().NotBeNull();
        statusFromResponse!.StatusName.Should().Be(statusName);
    }

    [Fact]
    public async Task ShouldFailToCreateStatus_WhenUnauthorized()
    {
        // Arrange
        const string statusName = "Unauthorized Status";
        var request = new CreateStatusDto(Name: statusName);

        // Act
        var response = await Client.PostAsJsonAsync("status/Create", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToCreateStatus_WhenNameIsEmpty()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var request = new CreateStatusDto(Name: string.Empty);

        // Act
        var response = await Client.PostAsJsonAsync("status/Create", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Name is required");
    }

    [Fact]
    public async Task ShouldFailToCreateStatus_WhenNameAlreadyExists()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var request = new CreateStatusDto(Name: _mainStatus.Name);

        // Act
        var response = await Client.PostAsJsonAsync("status/Create", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain($"A status with this id {_mainStatus.Id} is already exists");
    }

    public async Task InitializeAsync()
    {
        await Context.ProjectStatuses.AddRangeAsync(_mainStatus);
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
