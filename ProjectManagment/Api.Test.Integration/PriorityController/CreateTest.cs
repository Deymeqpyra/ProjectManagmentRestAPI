using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Api.Dtos.PrioritiesDto;
using Domain.Priorities;
using Domain.Roles;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.PriorityController;

public class CreateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;

    public CreateTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
    }

    [Fact]
    public async Task ShouldCreatePriority()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        const string priorityTitle = "High Priority";
        var request = new CreatePriorityDto(title: priorityTitle);

        // Act
        var response = await Client.PostAsJsonAsync("priorities/CreatePriority", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var priorityFromResponse = await response.Content.ReadFromJsonAsync<PriorityDto>();
        var priorityId = new ProjectPriorityId(priorityFromResponse.PriorityId!.Value);

        var priorityFromDataBase = await Context.ProjectPriorities.FirstOrDefaultAsync(x => x.Id == priorityId);

        priorityFromDataBase.Should().NotBeNull();
        priorityFromResponse!.Name.Should().Be(priorityTitle);
    }

    [Fact]
    public async Task ShouldFailToCreatePriority_WhenUnauthorized()
    {
        // Arrange
        const string priorityTitle = "Unauthorized Priority";
        var request = new CreatePriorityDto(title: priorityTitle);

        // Act
        var response = await Client.PostAsJsonAsync("priorities/CreatePriority", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToCreatePriority_WhenTitleIsEmpty()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var request = new CreatePriorityDto(title: string.Empty);

        // Act
        var response = await Client.PostAsJsonAsync("priorities/CreatePriority", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Name is required");
    }

    [Fact]
    public async Task ShouldFailToCreatePriority_WhenTitleIsTooLong()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var longTitle = new string('a', 101); // Assuming a 100-character limit
        var request = new CreatePriorityDto(title: longTitle);

        // Act
        var response = await Client.PostAsJsonAsync("priorities/CreatePriority", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Name is too long");
    }

    [Fact]
    public async Task ShouldFailToCreatePriority_WhenTitleAlreadyExists()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        const string duplicateTitle = "Duplicate Priority";
        var existingPriority = ProjectPriority.New(ProjectPriorityId.New(), duplicateTitle);
        await Context.ProjectPriorities.AddAsync(existingPriority);
        await SaveChangesAsync();

        var request = new CreatePriorityDto(title: duplicateTitle);

        // Act
        var response = await Client.PostAsJsonAsync("priorities/CreatePriority", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Priority with id:");
    }

    public async Task InitializeAsync()
    {
        await Context.Roles.AddRangeAsync(_adminRole);
        await Context.Users.AddRangeAsync(_adminUser);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Users.RemoveRange(Context.Users);
        Context.Roles.RemoveRange(Context.Roles);
        Context.ProjectPriorities.RemoveRange(Context.ProjectPriorities);
        await SaveChangesAsync();
    }
}
