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

public class UpdateTest : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Category _mainCategory;
    private readonly User _adminUser;
    private readonly Role _adminRole;

    public UpdateTest(IntegrationTestWebFactory factory) : base(factory)
    {
        _mainCategory = CategoryData.MainCategory();
        _adminRole = RoleData.AdminRole();
        _adminUser = UserData.AdminUser(_adminRole.Id);
    }

    [Fact]
    public async void ShouldCreateTest()
    {
        // arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        const string categoryUpdateName = "UpdateTest";

        var request = new CreateCategoryDto(Name: categoryUpdateName);

        // act 
        var response = await Client.PutAsJsonAsync($"categories/UpdateCategory/{_mainCategory.Id}", request);

        //assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var categoryFromResponse = await response.ToResponeModel<CategoryDto>();
        var categoryFromDataBase = await Context.Categories
            .FirstOrDefaultAsync(x => x.Id == new CategoryId(categoryFromResponse.CategoryId!.Value));

        categoryFromDataBase.Should().NotBeNull();
        categoryFromDataBase.Name.Should().Be(categoryUpdateName);
    }

    [Fact]
    public async void ShouldFailToUpdateCategory_WhenUnauthorized()
    {
        // Arrange
        const string categoryUpdateName = "UnauthorizedUpdate";
        var request = new CreateCategoryDto(Name: categoryUpdateName);

        // Act
        var response = await Client.PutAsJsonAsync($"categories/UpdateCategory/{_mainCategory.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }   

    [Fact]
    public async void ShouldFailToUpdateCategory_WhenCategoryDoesNotExist()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        const string categoryUpdateName = "NonExistentCategoryUpdate";
        var nonExistentCategoryId = Guid.NewGuid(); 
        var request = new CreateCategoryDto(Name: categoryUpdateName);

        // Act
        var response = await Client.PutAsJsonAsync($"categories/UpdateCategory/{nonExistentCategoryId}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async void ShouldFailToUpdateCategory_WhenNameIsEmpty()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var request = new CreateCategoryDto(Name: string.Empty);

        // Act
        var response = await Client.PutAsJsonAsync($"categories/UpdateCategory/{_mainCategory.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Name is required");
    }

    [Fact]
    public async void ShouldFailToUpdateCategory_WhenNameIsTooLong()
    {
        // Arrange
        var authToken = await GenerateAuthTokenAsync(_adminUser.Email, _adminUser.Password);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        var longCategoryName = new string('a', 256); // Assuming the max length is 255
        var request = new CreateCategoryDto(Name: longCategoryName);

        // Act
        var response = await Client.PutAsJsonAsync($"categories/UpdateCategory/{_mainCategory.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("Name is too long");
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