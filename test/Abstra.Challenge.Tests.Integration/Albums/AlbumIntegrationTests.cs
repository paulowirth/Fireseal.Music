using System.Net;
using System.Net.Http.Json;
using Abstra.Challenge.Application.Albums;
using Abstra.Challenge.Tests.Integration.Abstractions;
using Xunit.Abstractions;

namespace Abstra.Challenge.Tests.Integration.Albums;

public class AlbumIntegrationTests(AbstraApplicationFactory webApplicationFactory, ITestOutputHelper testOutputHelper) 
    : IntegrationTest(webApplicationFactory, testOutputHelper)
{
    [Fact]
    public async Task Albums_ShouldCreate_ReturnsExpected()
    {
        //Arrange
        var httpClient = WebApplicationFactory.CreateClient();
        
        await Authenticate(httpClient);

        var createRequest =
            new CreateAlbumRequest(
                "Test Album",
                "Test Artist",
                DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime),
                [new CreateAlbumTrackRequest("Test Track", "00:01:00", "TATT00000000")]);

        //Act
        var createResponse =
            await httpClient.PostAsJsonAsync("/abstra/albums", createRequest);

        createResponse.EnsureSuccessStatusCode();

        var albumId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var getResponse = await httpClient.GetAsync($"/abstra/albums/{albumId}");

        getResponse.EnsureSuccessStatusCode();

        var albumResponse = await getResponse.Content.ReadFromJsonAsync<AlbumResponse>();

        //Assert
        Assert.NotNull(albumResponse);
        Assert.Equal(createRequest.Title, albumResponse.Title);
        Assert.Equal(createRequest.Artist, albumResponse.Artist);
        Assert.Equal(createRequest.ReleaseDate, albumResponse.ReleaseDate);
        Assert.Equal(createRequest.Tracks.Count, albumResponse.Tracks.Count);
    }
    
    [Fact]
    public async Task Albums_ShouldUpdate_ReturnsExpected()
    {
        //Arrange
        var httpClient = WebApplicationFactory.CreateClient();
        
        await Authenticate(httpClient);

        var createRequest =
            new CreateAlbumRequest(
                "Test Album",
                "Test Artist",
                DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime),
                [new CreateAlbumTrackRequest("Test Track", "00:01:00", "TATT00000000")]);
        
        var updateRequest = 
            new UpdateAlbumRequest(
                "Updated Album", 
                "Updated Artist", 
                DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime.AddDays(-1)));

        //Act
        var createResponse =
            await httpClient.PostAsJsonAsync("/abstra/albums", createRequest);

        createResponse.EnsureSuccessStatusCode();

        var albumId = await createResponse.Content.ReadFromJsonAsync<Guid>();
        
        var updateResponse =
            await httpClient.PutAsJsonAsync($"/abstra/albums/{albumId}", updateRequest);
        
        updateResponse.EnsureSuccessStatusCode();

        var getResponse = await httpClient.GetAsync($"/abstra/albums/{albumId}");

        getResponse.EnsureSuccessStatusCode();

        var albumResponse = await getResponse.Content.ReadFromJsonAsync<AlbumResponse>();

        //Assert
        Assert.NotNull(albumResponse);
        Assert.Equal(updateRequest.Title, albumResponse.Title);
        Assert.Equal(updateRequest.Artist, albumResponse.Artist);
        Assert.Equal(updateRequest.ReleaseDate, albumResponse.ReleaseDate);
    }
    
    [Fact]
    public async Task Albums_ShouldDelete_ReturnsExpected()
    {
        //Arrange
        var httpClient = WebApplicationFactory.CreateClient();
        
        await Authenticate(httpClient);

        var createRequest =
            new CreateAlbumRequest(
                "Test Album",
                "Test Artist",
                DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime),
                [new CreateAlbumTrackRequest("Test Track", "00:01:00", "TATT00000000")]);

        //Act
        var createResponse =
            await httpClient.PostAsJsonAsync("/abstra/albums", createRequest);

        createResponse.EnsureSuccessStatusCode();

        var albumId = await createResponse.Content.ReadFromJsonAsync<Guid>();
        
        var deleteResponse = await httpClient.DeleteAsync($"/abstra/albums/{albumId}");
        
        deleteResponse.EnsureSuccessStatusCode();

        var getResponse = await httpClient.GetAsync($"/abstra/albums/{albumId}");

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
    
    [Fact]
    public async Task Albums_ShouldList_ReturnsExpected()
    {
        //Arrange
        var httpClient = WebApplicationFactory.CreateClient();
        
        await Authenticate(httpClient);
        
        //Act
        var response = await httpClient.GetAsync("/abstra/albums");
        response.EnsureSuccessStatusCode();
        
        //Assert
        var albums = await response.Content.ReadFromJsonAsync<List<AlbumResponse>>();
        
        Assert.NotNull(albums);
    }
}

