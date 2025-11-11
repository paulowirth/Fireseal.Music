namespace Fireseal.Music.Application.Albums;

public interface IAlbumService
{
    Task<AlbumResponse?> Get(Guid albumId, CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<AlbumResponse>> List(CancellationToken cancellationToken);
    
    Task<Guid> Create(CreateAlbumRequest request, CancellationToken cancellationToken);
    
    Task<bool> Update(Guid albumId, UpdateAlbumRequest request, CancellationToken cancellationToken);
    
    Task<bool> Delete(Guid albumId, CancellationToken cancellationToken);
}