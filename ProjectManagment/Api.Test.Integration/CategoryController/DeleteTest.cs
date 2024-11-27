using System.Net.Http.Headers;
using System.Net.Http.Json;
using Domain.Categories;
using Domain.Roles;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.CategoryController;

public class DeleteTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly User _adminUser;
    private readonly Category _mainCategory;
    private readonly Role _adminRole;

    public DeleteTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _mainCategory = CategoryData.MainCategory();
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
    }

    [Fact]
    public async void ShouldDeleteCategory()
    {
        //arrange 
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        
        // act 
        var response = await Client.DeleteAsync($"categories/DeleteCategory/{_mainCategory.Id}");
        
        //assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var categoryFromDatabase = await Context.Categories
            .FirstOrDefaultAsync(x => x.Id == _mainCategory.Id);

        categoryFromDatabase.Should().BeNull();
    }
    
    [Fact]
    public async void ShouldFailToDeleteCategory_WhenUnauthorized()
    {
        // Act
        var response = await Client.DeleteAsync($"categories/DeleteCategory/{_mainCategory.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async void ShouldFailToDeleteCategory_WhenCategoryDoesNotExist()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var nonExistentCategoryId = CategoryId.New(); 

        // Act
        var response = await Client.DeleteAsync($"categories/DeleteCategory/{nonExistentCategoryId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }


    public async Task InitializeAsync()
    {
        await Context.Categories.AddRangeAsync(_mainCategory);
        await Context.Roles.AddRangeAsync(_adminRole);
        await Context.Users.AddRangeAsync(_adminUser);
        await Context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Categories.RemoveRange(Context.Categories);
        Context.Users.RemoveRange(Context.Users);
        Context.Roles.RemoveRange(Context.Roles);
        await Context.SaveChangesAsync();
    }
}