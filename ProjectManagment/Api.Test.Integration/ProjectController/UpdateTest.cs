using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Api.Dtos.ProjectDto;
using Domain.Priorities;
using Domain.Projects;
using Domain.Roles;
using Domain.Statuses;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.ProjectController;

public class UpdateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private readonly Project _mainProject;
    private readonly Role _userRole;
    private readonly User _extraUser;
    private readonly ProjectPriority _mainPriority;
    private readonly ProjectStatus _mainStatus;

    public UpdateTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
        
        _userRole = RoleData.UserRole();
        _extraUser = UserData.ExtraUser(_userRole.Id);

        _mainStatus = StatusData.MainStatus();
        _mainPriority = PriorityData.MainPriority();
        _mainProject = ProjectData.MainProject(_extraUser.Id, _mainStatus.Id, _mainPriority.Id);
    }

    [Fact]
    public async Task ShouldUpdateProjectWithSuccess_WhenValidData()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var updateRequest = new UpdateProjectDto(
            Title: "Updated Project Title",
            Description: "Updated description for the project"
        );

        // Act
        var response = await Client.PutAsJsonAsync($"projects/update/{_mainProject.ProjectId}", updateRequest);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var updatedProject = await Context.Projects.FirstOrDefaultAsync(p => p.ProjectId == _mainProject.ProjectId);
        updatedProject.Should().NotBeNull();
        updatedProject.Title.Should().Be(updateRequest.Title);
        updatedProject.Description.Should().Be(updateRequest.Description);
    }

    [Fact]
    public async Task ShouldFailToUpdateProject_WhenUnauthorized()
    {
        // Arrange
        var updateRequest = new UpdateProjectDto(
            Title: "Unauthorized Update Title",
            Description: "Unauthorized update description"
        );

        // Act
        var response = await Client.PutAsJsonAsync($"projects/update/{_mainProject.ProjectId}", updateRequest);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToUpdateProject_WhenTitleIsEmpty()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var updateRequest = new UpdateProjectDto(
            Title: "", 
            Description: "Valid description"
        );

        // Act
        var response = await Client.PutAsJsonAsync($"projects/update/{_mainProject.ProjectId}", updateRequest);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldFailToUpdateProject_WhenProjectDoesNotExist()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var nonExistentProjectId = ProjectId.New();
        var updateRequest = new UpdateProjectDto(
            Title: "Non-existent Project",
            Description: "Description for non-existent project"
        );

        // Act
        var response = await Client.PutAsJsonAsync($"projects/update/{nonExistentProjectId}", updateRequest);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public async Task InitializeAsync()
    {
        await Context.Roles.AddRangeAsync(_adminRole, _userRole);
        await Context.Users.AddRangeAsync(_adminUser, _extraUser);
        await Context.ProjectStatuses.AddRangeAsync(_mainStatus);
        await Context.ProjectPriorities.AddRangeAsync(_mainPriority);
        await Context.Projects.AddRangeAsync(_mainProject);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Users.RemoveRange(Context.Users);
        Context.Roles.RemoveRange(Context.Roles);
        Context.ProjectStatuses.RemoveRange(Context.ProjectStatuses);
        Context.ProjectPriorities.RemoveRange(Context.ProjectPriorities);
        Context.Projects.RemoveRange(Context.Projects);
        await SaveChangesAsync();
    }
}
