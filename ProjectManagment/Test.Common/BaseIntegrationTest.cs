using System.Net.Http.Json;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Test.Common;

public class BaseIntegrationTest : IClassFixture<IntegrationTestWebFactory>
{
    protected readonly ApplicationDbContext Context;
    protected readonly HttpClient Client;

    protected BaseIntegrationTest(IntegrationTestWebFactory factory)
    {
        var scope = factory.Services.CreateScope();
        Context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        Client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
    }

    protected async Task<string> GenerateAuthTokenAsync(string email, string password)
    {
        var loginRequest = new {Email = email, Password = password};
        var response = await Client.PostAsJsonAsync("api/Auth/Login", loginRequest);
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    protected async Task<int> SaveChangesAsync()
    {
        var result = await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();
        return result;
    }
}