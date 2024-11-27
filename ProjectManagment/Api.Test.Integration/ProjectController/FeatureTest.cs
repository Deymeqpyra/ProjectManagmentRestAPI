using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Api.Dtos.CommentsDto;
using Domain.Comments;
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

public class FeatureTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;
    private readonly Project _mainProject;
    private readonly Role _userRole;
    private readonly User _extraUser;
    private readonly ProjectPriority _mainPriority;
    private readonly ProjectStatus _mainStatus;

    public FeatureTest(IntegrationTestWebFactory factory) : base(factory)
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
    public async void ShoudlAddCommentToProject()
    {
        // arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        
        var request = new CreateCommentDto(text:"Test comentary to add a new comment");
        
        // act
        var response = await Client.PostAsJsonAsync($"projects/addComment/{_mainProject.ProjectId}", request);
        
        
        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var commentFromResponse = await response.Content.ReadFromJsonAsync<CommentDto>();
        var commentId = new CommentId(commentFromResponse!.Id);
        
        var commentFromDb = await Context.Comments.FirstOrDefaultAsync(x=>x.Id == commentId);
        
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
        var request = new CreateCommentDto(text:"Test comentary to add a new comment");
        
        // act
        var response = await Client.PostAsJsonAsync($"projects/addComment/{projectId}", request);
        
        
        // assert
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
        Context.Comments.RemoveRange(Context.Comments);
        await SaveChangesAsync();
    }
}