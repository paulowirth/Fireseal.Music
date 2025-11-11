using System.Net.Http.Headers;
using System.Net.Http.Json;
using Fireseal.Music.Presentation.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Fireseal.Music.Tests.Integration.Abstractions;

public abstract class IntegrationTest : IClassFixture<FiresealApplicationFactory>, IAsyncLifetime
{
    protected WebApplicationFactory<Fireseal.Music.Api.Program> WebApplicationFactory { get; }

    protected IntegrationTest(FiresealApplicationFactory webApplicationFactory, ITestOutputHelper testOutputHelper)
    {
        var webAppFactory = 
            webApplicationFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                });
            });

        WebApplicationFactory = webAppFactory;
    }

    protected static async Task Authenticate(HttpClient httpClient)
    {
        var response =
            await httpClient.PostAsJsonAsync(
                "/authentication/token",
                new { Username = "admin", Password = "admin" });

        response.EnsureSuccessStatusCode();

        var authenticationResponse =
            await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authenticationResponse?.Token);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => Task.CompletedTask;
}