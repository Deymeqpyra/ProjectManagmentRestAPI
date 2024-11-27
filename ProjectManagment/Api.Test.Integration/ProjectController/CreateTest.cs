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

public class CreateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private readonly Project _mainProject;
    private readonly Role _userRole;
    private readonly User _extraUser;
    private readonly ProjectPriority _mainPriority;
    private readonly ProjectStatus _mainStatus;

    public CreateTest(IntegrationTestWebFactory factory) : base(factory)
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
    public async Task ShouldCreateProjectWithSuccess_WhenValidData()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        
        var request = new CreateProjectDto(
            Title: "New Project",
            Description: "Description for the new project",
            statusId: _mainStatus.Id.value,
            priorityId: _mainPriority.Id.value  
        );

        // Act
        var response = await Client.PostAsJsonAsync("projects/create", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var projectFromResponse = await response.Content.ReadFromJsonAsync<ProjectDto>();
        var projectId = new ProjectId(projectFromResponse.projectId!.Value);

        var projectFromDb = await Context.Projects.FirstOrDefaultAsync(x => x.ProjectId == projectId);

        projectFromDb.Should().NotBeNull();
        projectFromResponse.Title.Should().Be(request.Title);
        projectFromResponse.Description.Should().Be(request.Description);
    }

    [Fact]
    public async Task ShouldFailToCreateProject_WhenUnauthorized()
    {
        // Arrange
        var request = new CreateProjectDto(
            Title: "Unauthorized Project",
            Description: "Unauthorized project description",
            statusId: _mainStatus.Id.value,
            priorityId: _mainPriority.Id.value
        );

        // Act
        var response = await Client.PostAsJsonAsync("projects/create", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToCreateProject_WhenMissingRequiredFields()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        
        var request = new CreateProjectDto(
            Title: "", 
            Description: "Description for the project without title",
            statusId: _mainStatus.Id.value,
            priorityId: _mainPriority.Id.value
        );

        // Act
        var response = await Client.PostAsJsonAsync("projects/create", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldFailToCreateProject_WhenStatusDoesNotExist()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        
        var invalidStatusId = ProjectStatusId.New();
        var request = new CreateProjectDto(
            Title: "Project with Invalid Status",
            Description: "Description with invalid status",
            statusId: invalidStatusId.value,
            priorityId: _mainPriority.Id.value
        );

        // Act
        var response = await Client.PostAsJsonAsync("projects/create", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldFailToCreateProject_WhenPriorityDoesNotExist()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        
        var invalidPriorityId = ProjectPriorityId.New(); 
        var request = new CreateProjectDto(
            Title: "Project with Invalid Priority",
            Description: "Description with invalid priority",
            statusId: _mainStatus.Id.value,
            priorityId: invalidPriorityId.value
        );

        // Act
        var response = await Client.PostAsJsonAsync("projects/create", request);

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
