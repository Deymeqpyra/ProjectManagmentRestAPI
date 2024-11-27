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

public class UpdateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private readonly ProjectPriority _existingPriority;

    public UpdateTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
        _existingPriority = PriorityData.MainPriority();
    }

    [Fact]
    public async Task ShouldUpdatePriority()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email,UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var updatedTitle = "Updated Priority";
        var request = new CreatePriorityDto(updatedTitle);

        // Act
        var response = await Client.PutAsJsonAsync($"priorities/UpdatePriority/{_existingPriority.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var updatedPriority = await Context.ProjectPriorities.FirstOrDefaultAsync(x => x.Id == _existingPriority.Id);

        updatedPriority.Should().NotBeNull();
        updatedPriority!.Name.Should().Be(updatedTitle);
    }

    [Fact]
    public async Task ShouldFailToUpdatePriority_WhenUnauthorized()
    {
        // Arrange
        var updatedTitle = "Unauthorized Update";
        var request = new CreatePriorityDto(updatedTitle);

        // Act
        var response = await Client.PutAsJsonAsync($"priorities/UpdatePriority/{_existingPriority.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToUpdatePriority_WhenTitleIsEmpty()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var request = new CreatePriorityDto(string.Empty);

        // Act
        var response = await Client.PutAsJsonAsync($"priorities/UpdatePriority/{_existingPriority.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Name is required");
    }

    [Fact]
    public async Task ShouldFailToUpdatePriority_WhenTitleIsTooLong()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var longTitle = new string('a', 101);
        var request = new CreatePriorityDto( longTitle);

        // Act
        var response = await Client.PutAsJsonAsync($"priorities/UpdatePriority/{_existingPriority.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Name is too long");
    }


    public async Task InitializeAsync()
    {
        await Context.Roles.AddRangeAsync(_adminRole);
        await Context.Users.AddRangeAsync(_adminUser);
        await Context.ProjectPriorities.AddAsync(_existingPriority);
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
