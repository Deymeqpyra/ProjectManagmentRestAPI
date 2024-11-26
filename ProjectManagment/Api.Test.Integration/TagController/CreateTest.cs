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

public class CreateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private Tag _mainTag;

    public CreateTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _mainTag = TagData.MainTag();
        _adminUser = UserData.AdminUser(_adminRole.Id);
    }

    [Fact]
    public async Task ShouldCreateTag()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        const string tagName = "NewTag";
        var request = new CreateTagDto(Name: tagName);

        // Act
        var response = await Client.PostAsJsonAsync("tags/AddTag", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var tagFromResponse = await response.Content.ReadFromJsonAsync<TagDto>();
        var tagId = new TagId(tagFromResponse.tagId!.Value);

        var tagFromDatabase = await Context.Tags.FirstOrDefaultAsync(x => x.Id == tagId);

        tagFromDatabase.Should().NotBeNull();
        tagFromResponse!.name.Should().Be(tagName);
    }

    [Fact]
    public async Task ShouldFailToCreateTag_WhenUnauthorized()
    {
        // Arrange
        const string tagName = "UnauthorizedTag";
        var request = new CreateTagDto(Name: tagName);

        // Act
        var response = await Client.PostAsJsonAsync("tags/AddTag", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToCreateTag_WhenNameIsEmpty()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var request = new CreateTagDto(Name: string.Empty);

        // Act
        var response = await Client.PostAsJsonAsync("tags/AddTag", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Name is required");
    }

    [Fact]
    public async Task ShouldFailToCreateTag_WhenNameAlreadyExists()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var request = new CreateTagDto(Name: _mainTag.Name);

        // Act
        var response = await Client.PostAsJsonAsync("tags/AddTag", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain($"Tag already exists {_mainTag.Id.value}");
    }

    public async Task InitializeAsync()
    {
        await Context.Tags.AddRangeAsync(_mainTag);
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
