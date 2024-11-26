using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Api.Dtos.TagsDto;
using Domain.Tags;
using Domain.Roles;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.TagController;

public class UpdateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private readonly Tag _tagToUpdate;

    public UpdateTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
        _tagToUpdate = TagData.MainTag();
    }

    [Fact]
    public async Task ShouldUpdateTag()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var updatedName = "UpdatedTagName";
        var request = new CreateTagDto(Name: updatedName);

        // Act
        var response = await Client.PutAsJsonAsync($"tags/UpdateTag/{_tagToUpdate.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var tagFromDatabase = await Context.Tags.FirstOrDefaultAsync(x => x.Id == _tagToUpdate.Id);
        tagFromDatabase!.Name.Should().Be(updatedName);
    }

    [Fact]
    public async Task ShouldFailToUpdateTag_WhenUnauthorized()
    {
        // Arrange
        var updatedName = "UnauthorizedTagUpdate";
        var request = new CreateTagDto( Name: updatedName);

        // Act
        var response = await Client.PutAsJsonAsync($"tags/UpdateTag/{_tagToUpdate.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    [Fact]
    public async Task ShouldFailToUpdateTag_WhenIdDoesNotExist()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var nonExistentRoleId = TagId.New();
        var request = new CreateTagDto(Name: "Non-existent Role");

        // Act
        var response = await Client.PutAsJsonAsync($"tags/UpdateTag/{nonExistentRoleId}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain($"Tag with {nonExistentRoleId} not found");
    }

    public async Task InitializeAsync()
    {
        await Context.Tags.AddRangeAsync(_tagToUpdate);
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
