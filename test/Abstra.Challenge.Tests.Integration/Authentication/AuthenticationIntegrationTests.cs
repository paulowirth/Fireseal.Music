using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Abstra.Challenge.Presentation.Authentication;
using Abstra.Challenge.Tests.Integration.Abstractions;
using Xunit.Abstractions;

namespace Abstra.Challenge.Tests.Integration.Authentication;

public class AuthenticationIntegrationTests(AbstraApplicationFactory factory, ITestOutputHelper testOutputHelper) 
    : IntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task Token_ShouldGenerate_ReturnsExpected()
    {
        //Arrange
        var httpClient = WebApplicationFactory.CreateClient();
        
        //Act
        var response = 
            await httpClient.PostAsJsonAsync(
                "/authentication/token", 
                new { Username = "admin", Password = "admin" });
        
        response.EnsureSuccessStatusCode();
        
        var authenticationResponse = 
            await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

        //Assert
        Assert.NotNull(authenticationResponse);
        Assert.NotEmpty(authenticationResponse.Token);
    }

    [Fact]
    public async Task Token_ShouldUnauthorize_InvalidCredentials()
    {
        //Arrange
        var httpClient = WebApplicationFactory.CreateClient();
        
        //Act
        var response = 
            await httpClient.PostAsJsonAsync(
                "/authentication/token", 
                new { Username = "admin", Password = "wrong" });
        
        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Albums_ShouldNotList_WhenUserIsNotAuthenticated()
    {
        //Arrange
        var httpClient = WebApplicationFactory.CreateClient();
        
        //Act
        var response = await httpClient.GetAsync("/abstra/albums");
        
        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Albums_ShouldList_WhenUserIsAuthenticated()
    {
        //Arrange
        var httpClient = WebApplicationFactory.CreateClient();
        
        var tokenResponse = 
            await httpClient.PostAsJsonAsync(
                "/authentication/token", 
                new { Username = "admin", Password = "admin" });
        
        tokenResponse.EnsureSuccessStatusCode();
        
        var authenticationResponse = 
            await tokenResponse.Content.ReadFromJsonAsync<AuthenticationResponse>();
        
        //Act
        httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", authenticationResponse?.Token);
        
        var response = await httpClient.GetAsync("/abstra/albums");
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

