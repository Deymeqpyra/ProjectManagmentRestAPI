using System.Net;
using System.Net.Http.Headers;
using Domain.Priorities;
using Domain.Roles;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.PriorityController;

public class DeleteTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private readonly ProjectPriority _existingPriority;

    public DeleteTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
        _existingPriority = PriorityData.MainPriority();
    }

    [Fact]
    public async Task ShouldDeletePriority()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        // Act
        var response = await Client.DeleteAsync($"priorities/DeletePriority/{_existingPriority.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var deletedPriority = await Context.ProjectPriorities.FirstOrDefaultAsync(x => x.Id == _existingPriority.Id);
        deletedPriority.Should().BeNull();
    }

    [Fact]
    public async Task ShouldFailToDeletePriority_WhenUnauthorized()
    {
        // Act
        var response = await Client.DeleteAsync($"priorities/DeletePriority/{_existingPriority.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToDeletePriority_WhenPriorityDoesNotExist()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var nonExistentId = ProjectPriorityId.New();

        // Act
        var response = await Client.DeleteAsync($"priorities/DeletePriority/{nonExistentId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Priority with id: ");
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
