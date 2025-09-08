using Abstra.Challenge.Domain;
using Abstra.Challenge.Domain.Abstractions;

namespace Abstra.Challenge.Application.Tracks;

internal sealed class TrackService : ITrackService
{
    private readonly IRepository<Track, Guid> _repository;
    
    public TrackService(IRepository<Track, Guid> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }
    
    public async Task<TrackResponse?> Get(Guid trackId, CancellationToken cancellationToken)
    {
        var track = await _repository.Get(trackId, [track => track.Album], cancellationToken);
        
        return track?.ToResponse();
    }

    public async Task<IReadOnlyCollection<TrackResponse>> List(CancellationToken cancellationToken)
    {
        var tracks = await _repository.List([track => track.Album], cancellationToken);
        
        return tracks
            .Select(track => track.ToResponse())
            .OrderBy(track => track.Album)
            .ToList();
    }

    public async Task<Guid> Create(SaveTrackRequest createRequest, CancellationToken cancellationToken)
    {
        var trackToCreate = createRequest.ToDomain();
        
        await _repository.Insert(trackToCreate, cancellationToken);

        return trackToCreate.Id;
    }

    public async Task<bool> Update(Guid albumId, SaveTrackRequest updateRequest, CancellationToken cancellationToken)
    {
        var trackToUpdate = updateRequest.ToDomain(albumId);
        
        return await _repository.Update(trackToUpdate, [], cancellationToken);
    }

    public async Task<bool> Delete(Guid trackId, CancellationToken cancellationToken) => 
        await _repository.Delete(trackId, cancellationToken);
}