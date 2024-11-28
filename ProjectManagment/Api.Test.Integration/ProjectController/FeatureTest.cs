using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Api.Dtos.CommentsDto;
using Api.Dtos.TasksDto;
using Domain.Categories;
using Domain.Comments;
using Domain.Priorities;
using Domain.Projects;
using Domain.Roles;
using Domain.Statuses;
using Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.ProjectController;

public class FeatureTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private readonly Project _mainProject;
    private readonly Role _userRole;
    private readonly User _extraUser;
    private readonly User _megaExtraUser;
    private readonly ProjectPriority _mainPriority;
    private readonly ProjectStatus _mainStatus;
    private readonly Project _extraProject;
    private readonly ProjectStatus _extraStatus;
    private readonly ProjectPriority _extraPriority;
    private readonly Role _userExtraRole;
    private readonly Category _mainCategory;

    public FeatureTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);

        _userRole = RoleData.UserRole();
        _userExtraRole = RoleData.ExtraRole();
        
        _extraUser = UserData.ExtraUser(_userRole.Id);
        _megaExtraUser = UserData.MegaExtra(_userExtraRole.Id);

        _mainCategory = CategoryData.ExtraCategory();
        
        _mainStatus = StatusData.MainStatus();
        _extraStatus = StatusData.ExtraStatus();
        _mainPriority = PriorityData.MainPriority();
        _extraPriority = PriorityData.ExtraPriority();
        
        _mainProject = ProjectData.MainProject(_extraUser.Id, _mainStatus.Id, _mainPriority.Id);
        _extraProject = ProjectData.MainProject(_megaExtraUser.Id, _extraStatus.Id, _extraPriority.Id);
    }

    [Fact]
    public async void ShoudlAddCommentToProject()
    {
        // arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var request = new CreateCommentDto(text: "Test comentary to add a new comment");

        // act
        var response = await Client.PostAsJsonAsync($"projects/addComment/{_mainProject.ProjectId}", request);


        // assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var commentFromResponse = await response.Content.ReadFromJsonAsync<CommentDto>();
        var commentId = new CommentId(commentFromResponse!.Id);

        var commentFromDb = await Context.Comments.FirstOrDefaultAsync(x => x.Id == commentId);

        commentFromDb.Should().NotBeNull();
        commentFromDb.Content.Should().Be(request.text);
    }

    [Fact]
    public async void ShouldFailToAddCommentToProject_CauseNotFoundProject()
    {
        // arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var projectId = ProjectId.New();
        var request = new CreateCommentDto(text: "Test comentary to add a new comment");

        // act
        var response = await Client.PostAsJsonAsync($"projects/addComment/{projectId}", request);


        // assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void ShouldAddUserToProject()
    {
        // arrange
        var authToken = await GenerateAuthTokenAsync(_megaExtraUser.Email, UserData.passwordUser);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);


        // act
        var response = await Client.PutAsync($"projects/addUser/{_extraUser.Id}/toProject/{_extraProject.ProjectId}", null);


        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var userInProject = await Context.ProjectUsers
            .FirstOrDefaultAsync(x => x.ProjectId == _extraProject.ProjectId && x.UserId == _extraUser.Id);

        userInProject.Should().NotBeNull();
    }
    [Fact]
    public async void ShouldAddTaskToProject()
    {
        // arrange
        var authToken = await GenerateAuthTokenAsync(_megaExtraUser.Email, UserData.passwordUser);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        const string title = "Test title to add a new task";
        const string desc = "Test desc to add a new task";
        var request = new CreateTaskDto(
            title: title,
            description: desc,
            categoryId: _mainCategory.Id.Value);

        // act
        var response = await Client.PostAsJsonAsync($"tasks/CreateTask/{_extraProject.ProjectId.value}", request);


        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var tasksResponse = await response.Content.ReadFromJsonAsync<CreateTaskDto>();
        tasksResponse.Should().NotBeNull();
        tasksResponse.title.Should().Be(title);
        tasksResponse.description.Should().Be(desc);
    }

    public async Task InitializeAsync()
    {
        await Context.Categories.AddRangeAsync(_mainCategory);
        await Context.Roles.AddRangeAsync(_adminRole, _userRole, _userExtraRole);
        await Context.Users.AddRangeAsync(_adminUser, _extraUser, _megaExtraUser);
        await Context.ProjectStatuses.AddRangeAsync(_mainStatus, _extraStatus);
        await Context.ProjectPriorities.AddRangeAsync(_mainPriority, _extraPriority);
        await Context.Projects.AddRangeAsync(_mainProject, _extraProject);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Categories.RemoveRange(Context.Categories);
        Context.Users.RemoveRange(Context.Users);
        Context.Roles.RemoveRange(Context.Roles);
        Context.ProjectStatuses.RemoveRange(Context.ProjectStatuses);
        Context.ProjectPriorities.RemoveRange(Context.ProjectPriorities);
        Context.Projects.RemoveRange(Context.Projects);
        Context.ProjectUsers.RemoveRange(Context.ProjectUsers);
        Context.ProjectTasks.RemoveRange(Context.ProjectTasks);
        Context.Comments.RemoveRange(Context.Comments);
        await SaveChangesAsync();
    }
}