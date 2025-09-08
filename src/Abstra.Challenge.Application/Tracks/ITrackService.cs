namespace Abstra.Challenge.Application.Tracks;

public interface ITrackService
{
    Task<TrackResponse?> Get(Guid trackId, CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<TrackResponse>> List(CancellationToken cancellationToken);
    
    Task<Guid> Create(SaveTrackRequest createRequest, CancellationToken cancellationToken);
    
    Task<bool> Update(Guid albumId, SaveTrackRequest updateRequest, CancellationToken cancellationToken);
    
    Task<bool> Delete(Guid trackId, CancellationToken cancellationToken);
}