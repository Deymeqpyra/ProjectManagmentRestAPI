using System.Net;
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

namespace Api.Test.Integration.CategoryController;

public class CreateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Role _adminRole;
    private readonly User _adminUser;

    public CreateTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
    }


    [Fact]
    public async Task ShouldCreateCategory()
    {
        //Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        const string categoryName = "test_category";
        var request = new CreateCategoryDto(Name: categoryName);

        //Act
        var response = await Client.PostAsJsonAsync("categories/CreateCategory", request);

        //Arrange
        response.IsSuccessStatusCode.Should().BeTrue();

        var categoryFromResponse = await response.Content.ReadFromJsonAsync<CategoryDto>();
        var categoryId = new CategoryId(categoryFromResponse.CategoryId!.Value);

        var categoryFromDataBase = await Context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

        categoryFromDataBase.Should().NotBeNull();

        categoryFromResponse!.Name.Should().Be(categoryName);
    }

    [Fact]
    public async Task ShouldFailToCreateCategory_WhenUnauthorized()
    {
        // Arrange
        const string categoryName = "unauthorized_category";
        var request = new CreateCategoryDto(Name: categoryName);

        // Act
        var response = await Client.PostAsJsonAsync("categories/CreateCategory", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldFailToCreateCategory_WhenNameIsEmpty()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var request = new CreateCategoryDto(Name: string.Empty);

        // Act
        var response = await Client.PostAsJsonAsync("categories/CreateCategory", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Name is required");
    }

    [Fact]
    public async Task ShouldFailToCreateCategory_WhenNameIsTooLong()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var longCategoryName = new string('a', 56);
        var request = new CreateCategoryDto(Name: longCategoryName);

        // Act
        var response = await Client.PostAsJsonAsync("categories/CreateCategory", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Name is too long");
    }

    [Fact]
    public async Task ShouldFailToCreateCategory_WhenNameAlreadyExists()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        const string duplicateCategoryName = "duplicate_category";
        var existingCategory = Category.New(CategoryId.New(), duplicateCategoryName);
        await Context.Categories.AddAsync(existingCategory);
        await SaveChangesAsync();

        var request = new CreateCategoryDto(Name: duplicateCategoryName);

        // Act
        var response = await Client.PostAsJsonAsync("categories/CreateCategory", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Category with id:");
    }

    [Fact]
    public async Task ShouldCreateCategory_WhenValidRequest()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, UserData.passwordAdmin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        const string categoryName = "valid_category";
        var request = new CreateCategoryDto(Name: categoryName);

        // Act
        var response = await Client.PostAsJsonAsync("categories/CreateCategory", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var categoryFromResponse = await response.Content.ReadFromJsonAsync<CategoryDto>();
        var categoryId = new CategoryId(categoryFromResponse.CategoryId!.Value);

        var categoryFromDataBase = await Context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

        categoryFromDataBase.Should().NotBeNull();
        categoryFromResponse!.Name.Should().Be(categoryName);
    }


    public async Task InitializeAsync()
    {
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