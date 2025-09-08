using Abstra.Challenge.Domain;
using Abstra.Challenge.Domain.Abstractions;

namespace Abstra.Challenge.Application.Albums;

internal sealed class AlbumService : IAlbumService
{
    private readonly IRepository<Album, Guid> _repository;
    
    public AlbumService(IRepository<Album, Guid> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    public async Task<AlbumResponse?> Get(Guid albumId, CancellationToken cancellationToken)
    {
        var album = await _repository.Get(albumId, [album => album.Tracks], cancellationToken);
        
        return album?.ToResponse();
    }

    public async Task<IReadOnlyCollection<AlbumResponse>> List(CancellationToken cancellationToken)
    {
        var albums = await _repository.List([], cancellationToken);
        
        return albums
            .Select(album => album.ToResponse())
            .ToList();
    }

    public async Task<Guid> Create(CreateAlbumRequest request, CancellationToken cancellationToken)
    {
        var album = request.ToDomain();
        
        await _repository.Insert(album, cancellationToken);

        return album.Id;
    }

    public async Task<bool> Update(Guid albumId, UpdateAlbumRequest request, CancellationToken cancellationToken)
    {
        var albumToUpdate = request.ToDomain(albumId);
        
        return await _repository.Update(albumToUpdate, [album => album.Tracks], cancellationToken);
    }

    public async Task<bool> Delete(Guid albumId, CancellationToken cancellationToken) => 
        await _repository.Delete(albumId, cancellationToken);
}