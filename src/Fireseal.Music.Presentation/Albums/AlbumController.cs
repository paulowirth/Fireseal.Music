using Fireseal.Music.Application.Albums;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fireseal.Music.Presentation.Albums;

/// <summary>
/// Handles HTTP requests related to albums.
/// </summary>
/// <response code="400">
///     If the client request is not valid or malformed, returns a response with the error details.
/// </response>
/// <response code="500">
///     If an unexpected error occurs, returns a response with the error details.
/// </response>
[Authorize]
[ApiController]
[Route("fireseal/albums")]
public class AlbumController : ControllerBase
{
    /// <summary>
    /// Retrieves all albums.
    /// </summary>
    /// <param name="albumService">
    ///     Injected album service to handle business logic.
    /// </param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    ///   An <see cref="IActionResult"/> containing a list of <see cref="AlbumResponse"/> objects.
    /// </returns>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<AlbumResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromServices] IAlbumService albumService, 
        CancellationToken cancellationToken)
    {
        var listResponse = await albumService.List(cancellationToken);
  
        return Ok(listResponse);
    }
    
    /// <summary>
    /// Retrieves a specific album by its unique identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the album to retrieve.
    /// </param>
    /// <param name="albumService">
    ///     Injected album service to handle business logic.
    /// </param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    ///     An <see cref="IActionResult"/> containing the <see cref="AlbumResponse"/> object if found;
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<AlbumResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id,
        [FromServices] IAlbumService albumService, 
        CancellationToken cancellationToken)
    {
        var getResponse = await albumService.Get(id, cancellationToken);
  
        return getResponse is null 
            ? NotFound() 
            : Ok(getResponse);
    }

    /// <summary>
    /// Creates a new album with all of its tracks.
    /// </summary>
    /// <param name="createRequest">
    ///     The request payload containing album details and its tracks.
    /// </param>
    /// <param name="albumService">
    ///     The injected album service to handle business logic.
    /// </param>
    /// <param name="validator">
    ///     The injected validator to validate the incoming request.
    /// </param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    ///     An <see cref="IActionResult"/> containing the unique identifier of the newly created album;
    /// </returns>
    [HttpPost]
    [ProducesResponseType<Guid>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAlbumRequest createRequest,
        [FromServices] IAlbumService albumService,
        [FromServices] IValidator<CreateAlbumRequest> validator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createRequest, cancellationToken);
        
        var albumId = await albumService.Create(createRequest, cancellationToken);
        
        return CreatedAtAction(nameof(Get), new { id = albumId }, albumId);
    }

    /// <summary>
    /// Updates an existing album metadata.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the album to update.
    /// </param>
    /// <param name="updateRequest">
    ///     The request payload containing album details and its tracks.
    /// </param>
    /// <param name="albumService">
    ///     The injected album service to handle business logic.
    /// </param>
    /// <param name="validator">
    ///     The injected validator to validate the incoming request.
    /// </param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateAlbumRequest updateRequest,
        [FromServices] IAlbumService albumService,
        [FromServices] IValidator<UpdateAlbumRequest> validator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(updateRequest, cancellationToken);

        var updated = await albumService.Update(id, updateRequest, cancellationToken);

        return updated 
            ? NoContent() 
            : NotFound();
    }
    
    /// <summary>
    /// Deletes a specific album by its unique identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the album to delete.
    /// </param>
    /// <param name="albumService">
    ///     Injected album service to handle business logic.
    /// </param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    ///     An <see cref="IActionResult"/> containing the response of the request;
    /// </returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] IAlbumService albumService, 
        CancellationToken cancellationToken)
    {
        return await albumService.Delete(id, cancellationToken)
            ? Ok() 
            : NotFound();
    }
}