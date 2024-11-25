using System.Net.Http.Headers;
using System.Net.Http.Json;
using Api.Dtos.CategoriesDto;
using Domain.Categories;
using Domain.Roles;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;
using Test.Data;
using Xunit;

namespace Api.Test.Integration.Categories;

public class CreateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Category _mainCategory;
    private readonly Role _adminRole;
    private readonly User _adminUser;

    public CreateTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _mainCategory = CategoryData.MainCategory();
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
    }

    [Fact]
    public async Task ShouldCreateCategory()
    {
        //Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        const string categoryName = "test_category";
        var request = new CreateCategoryDto(Name:categoryName);
        
        //Act
        var response = await Client.PostAsJsonAsync("categories/CreateCategory", request);
        
        //Arrange
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var categoryFromResponse = await response.Content.ReadFromJsonAsync<CategoryDto>();
        var categoryId = new CategoryId(categoryFromResponse.CategoryId!.Value);
        
        var categoryFromDataBase = await Context.Categories.FirstOrDefaultAsync(x=>x.Id == categoryId);
        
        categoryFromDataBase.Should().NotBeNull();
            
        categoryFromResponse!.Name.Should().Be(categoryName);
    }

    public async Task InitializeAsync()
    {
        await Context.Categories.AddRangeAsync(_mainCategory);
        await Context.Roles.AddRangeAsync(_adminRole);
        await Context.Users.AddRangeAsync(_adminUser);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Users.RemoveRange(Context.Users);
        Context.Roles.RemoveRange(Context.Roles);
        Context.Categories.RemoveRange(Context.Categories);
        await SaveChangesAsync();
    }
}