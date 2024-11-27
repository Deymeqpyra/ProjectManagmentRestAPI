using System.Net;
using System.Net.Http.Headers;
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

public class DeleteTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private readonly Project _mainProject;
    private readonly Role _userRole;
    private readonly User _extraUser;
    private readonly ProjectPriority _mainPriority;
    private readonly ProjectStatus _mainStatus;

    public DeleteTest(IntegrationTestWebFactory factory) : base(factory)
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
    public async Task ShouldDeleteProjectWithSuccess_WhenAuthorized()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        // Act
        var response = await Client.DeleteAsync($"projects/deleteProject/{_mainProject.ProjectId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var deletedProject = await Context.Projects.FirstOrDefaultAsync(p => p.ProjectId == _mainProject.ProjectId);
        deletedProject.Should().BeNull(); 
    }

    [Fact]
    public async Task ShouldFailToDeleteProject_WhenUnauthorized()
    {
        // Arrange
        var projectToDelete = await Context.Projects.FirstOrDefaultAsync(p => p.ProjectId == _mainProject.ProjectId);
        projectToDelete.Should().NotBeNull();

        // Act
        var response = await Client.DeleteAsync($"projects/deleteProject/{_mainProject.ProjectId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToDeleteProject_WhenProjectDoesNotExist()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var nonExistentProjectId = ProjectId.New(); 

        // Act
        var response = await Client.DeleteAsync($"projects/deleteProject/{nonExistentProjectId}");

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
