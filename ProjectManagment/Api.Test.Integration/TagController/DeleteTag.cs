using System.Net;
using System.Net.Http.Headers;
using Domain.Tags;
using Domain.Roles;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.TagController;

public class DeleteTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private readonly Tag _tagToDelete;

    public DeleteTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
        _tagToDelete = TagData.MainTag();
    }

    [Fact]
    public async Task ShouldDeleteTag()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        // Act
        var response = await Client.DeleteAsync($"tags/DeleteTag/{_tagToDelete.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var tagFromDatabase = await Context.Tags.FirstOrDefaultAsync(x => x.Id == _tagToDelete.Id);
        tagFromDatabase.Should().BeNull();
    }

    [Fact]
    public async Task ShouldFailToDeleteTag_WhenTagDoesNotExist()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var nonExistentTagId = TagId.New();

        // Act
        var response = await Client.DeleteAsync($"tags/DeleteTag/{nonExistentTagId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain($"Tag with {nonExistentTagId} not found");
    }
    
    [Fact]
    public async Task ShouldFailToDeleteTag_WhenUnauthorized()
    {
        // Act
        var response = await Client.DeleteAsync($"tags/DeleteTag/{_tagToDelete.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    public async Task InitializeAsync()
    {
        await Context.Tags.AddRangeAsync(_tagToDelete);
        await Context.Roles.AddRangeAsync(_adminRole);
        await Context.Users.AddRangeAsync(_adminUser);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Users.RemoveRange(Context.Users);
        Context.Roles.RemoveRange(Context.Roles);
        Context.Tags.RemoveRange(Context.Tags);
        await SaveChangesAsync();
    }
}
