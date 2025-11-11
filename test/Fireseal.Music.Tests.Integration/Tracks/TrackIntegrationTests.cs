using System.Net;
using System.Net.Http.Json;
using Fireseal.Music.Application.Albums;
using Fireseal.Music.Application.Tracks;
using Fireseal.Music.Tests.Integration.Abstractions;
using Xunit.Abstractions;

namespace Fireseal.Music.Tests.Integration.Tracks;

public class TrackIntegrationTests(FiresealApplicationFactory webApplicationFactory, ITestOutputHelper testOutputHelper) 
    : IntegrationTest(webApplicationFactory, testOutputHelper)
{
    [Fact]
    public async Task Tracks_ShouldCreate_ReturnsExpected()
    {
        //Arrange
        var httpClient = WebApplicationFactory.CreateClient();
        
        await Authenticate(httpClient);

        var createAlbumRequest =
            new CreateAlbumRequest(
                "Test Album",
                "Test Artist",
                DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime),
                [new CreateAlbumTrackRequest("Test Track", "00:01:00", "TATT00000000")]);

        //Act
        var createAlbumResponse =
            await httpClient.PostAsJsonAsync("/fireseal/albums", createAlbumRequest);

        createAlbumResponse.EnsureSuccessStatusCode();

        var albumId = await createAlbumResponse.Content.ReadFromJsonAsync<Guid>();
        
        var createTrackRequest =
            new SaveTrackRequest(
                albumId,
                "New Track",
                "00:02:00",
                "TATT00000001");
        
        var createTrackResponse =
            await httpClient.PostAsJsonAsync("/fireseal/tracks", createTrackRequest);
        
        createTrackResponse.EnsureSuccessStatusCode();

        var trackId = await createTrackResponse.Content.ReadFromJsonAsync<Guid>();

        var getTrackResponse = await httpClient.GetAsync($"/fireseal/tracks/{trackId}");

        getTrackResponse.EnsureSuccessStatusCode();

        var trackResponse = await getTrackResponse.Content.ReadFromJsonAsync<TrackResponse>();

        //Assert
        Assert.NotNull(trackResponse);
        Assert.Equal(createTrackRequest.Title, trackResponse.Title);
        Assert.Equal(createTrackRequest.Duration, trackResponse.Duration);
        Assert.Equal(createTrackRequest.Isrc, trackResponse.Isrc);
        Assert.Equal(createAlbumRequest.Title, trackResponse.Album);
        Assert.Equal(createAlbumRequest.Artist, trackResponse.Artist);
    }
    
    [Fact]
    public async Task Tracks_ShouldUpdate_ReturnsExpected()
    {
        //Arrange
        var httpClient = WebApplicationFactory.CreateClient();
        
        await Authenticate(httpClient);

        var createAlbumRequest =
            new CreateAlbumRequest(
                "Test Album",
                "Test Artist",
                DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime),
                [new CreateAlbumTrackRequest("Test Track", "00:01:00", "TATT00000000")]);

        //Act
        var createAlbumResponse =
            await httpClient.PostAsJsonAsync("/fireseal/albums", createAlbumRequest);

        createAlbumResponse.EnsureSuccessStatusCode();

        var albumId = await createAlbumResponse.Content.ReadFromJsonAsync<Guid>();
        
        var createTrackRequest =
            new SaveTrackRequest(
                albumId,
                "New Track",
                "00:02:00",
                "TATT00000001");
        
        var createTrackResponse =
            await httpClient.PostAsJsonAsync("/fireseal/tracks", createTrackRequest);
        
        createTrackResponse.EnsureSuccessStatusCode();

        var trackId = await createTrackResponse.Content.ReadFromJsonAsync<Guid>();
        
        var updateTrackRequest =
            new SaveTrackRequest(
                albumId,
                "Updated Track Title",
                "00:03:00",
                "TATT00000002");
        
        var updateTrackResponse =
            await httpClient.PutAsJsonAsync($"/fireseal/tracks/{trackId}", updateTrackRequest);

        updateTrackResponse.EnsureSuccessStatusCode();
        
        var getTrackResponse = await httpClient.GetAsync($"/fireseal/tracks/{trackId}");

        getTrackResponse.EnsureSuccessStatusCode();

        var trackResponse = await getTrackResponse.Content.ReadFromJsonAsync<TrackResponse>();

        //Assert
        Assert.NotNull(trackResponse);
        Assert.Equal(updateTrackRequest.Title, trackResponse.Title);
        Assert.Equal(updateTrackRequest.Duration, trackResponse.Duration);
        Assert.Equal(updateTrackRequest.Isrc, trackResponse.Isrc);
        Assert.Equal(createAlbumRequest.Title, trackResponse.Album);
        Assert.Equal(createAlbumRequest.Artist, trackResponse.Artist);
    }
    
    [Fact]
    public async Task Tracks_ShouldDelete_ReturnsExpected()
    {
        //Arrange
        var httpClient = WebApplicationFactory.CreateClient();
        
        await Authenticate(httpClient);
    
        var createAlbumRequest =
            new CreateAlbumRequest(
                "Test Album",
                "Test Artist",
                DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime),
                [new CreateAlbumTrackRequest("Test Track", "00:01:00", "TATT00000000")]);
    
        //Act
        var createAlbumResponse =
            await httpClient.PostAsJsonAsync("/fireseal/albums", createAlbumRequest);
    
        createAlbumResponse.EnsureSuccessStatusCode();
    
        var albumId = await createAlbumResponse.Content.ReadFromJsonAsync<Guid>();
        
        var createTrackRequest =
            new SaveTrackRequest(
                albumId,
                "New Track",
                "00:02:00",
                "TATT00000001");
        
        var createTrackResponse =
            await httpClient.PostAsJsonAsync("/fireseal/tracks", createTrackRequest);
        
        createTrackResponse.EnsureSuccessStatusCode();
    
        var trackId = await createTrackResponse.Content.ReadFromJsonAsync<Guid>();
        
        //Act
        var deleteTrackResponse =
            await httpClient.DeleteAsync($"/fireseal/tracks/{trackId}");
    
        deleteTrackResponse.EnsureSuccessStatusCode();
        
        var getTrackResponse = await httpClient.GetAsync($"/fireseal/tracks/{trackId}");
    
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, getTrackResponse.StatusCode);
    }
    
    [Fact]
    public async Task Albums_ShouldList_ReturnsExpected()
    {
        //Arrange
        var httpClient = WebApplicationFactory.CreateClient();
        
        await Authenticate(httpClient);
        
        //Act
        var listResponse = await httpClient.GetAsync("/fireseal/tracks");
        listResponse.EnsureSuccessStatusCode();
        
        //Assert
        var albums = await listResponse.Content.ReadFromJsonAsync<List<TrackResponse>>();
        
        Assert.NotNull(albums);
    }
}

