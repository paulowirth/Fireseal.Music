using Fireseal.Music.Application.Tracks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fireseal.Music.Presentation.Tracks;

/// <summary>
/// Handles HTTP requests related to tracks.
/// </summary>
/// <response code="400">
///     If the client request is not valid or malformed, returns a response with the error details.
/// </response>
/// <response code="500">
///     If an unexpected error occurs, returns a response with the error details.
/// </response>
[ApiController]
[Route("fireseal/tracks")]
public class TrackController : ControllerBase
{
    /// <summary>
    /// Retrieves all tracks.
    /// </summary>
    /// <param name="trackService">
    ///     Injected track service to handle business logic.
    /// </param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    ///   An <see cref="IActionResult"/> containing a list of <see cref="TrackResponse"/> objects.
    /// </returns>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<TrackResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromServices] ITrackService trackService, 
        CancellationToken cancellationToken)
    {
        var listResponse = await trackService.List(cancellationToken);
  
        return Ok(listResponse);
    }
    
    /// <summary>
    /// Retrieves a specific track by its unique identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the track to retrieve.
    /// </param>
    /// <param name="trackService">
    ///     Injected track service to handle business logic.
    /// </param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    ///     An <see cref="IActionResult"/> containing the <see cref="TrackResponse"/> object if found;
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<TrackResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id,
        [FromServices] ITrackService trackService, 
        CancellationToken cancellationToken)
    {
        var getResponse = await trackService.Get(id, cancellationToken);
  
        return getResponse is null 
            ? NotFound() 
            : Ok(getResponse);
    }

    /// <summary>
    /// Adds a new track for an existing album.
    /// </summary>
    /// <param name="createRequest">
    ///     The request payload containing album details and its tracks.
    /// </param>
    /// <param name="trackService">
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
        [FromBody] SaveTrackRequest createRequest,
        [FromServices] ITrackService trackService,
        [FromServices] IValidator<SaveTrackRequest> validator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createRequest, cancellationToken);
        
        var trackId = await trackService.Create(createRequest, cancellationToken);
        
        return CreatedAtAction(nameof(Get), new { id = trackId }, trackId);
    }

    /// <summary>
    /// Updates an existing track metadata.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the track to update.
    /// </param>
    /// <param name="updateRequest">
    ///     The request payload containing track details.
    /// </param>
    /// <param name="trackService">
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
        [FromBody] SaveTrackRequest updateRequest,
        [FromServices] ITrackService trackService,
        [FromServices] IValidator<SaveTrackRequest> validator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(updateRequest, cancellationToken);

        var updated = await trackService.Update(id, updateRequest, cancellationToken);

        return updated 
            ? NoContent() 
            : NotFound();
    }
    
    /// <summary>
    /// Deletes a specific track by its unique identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the track to delete.
    /// </param>
    /// <param name="trackService">
    ///     Injected track service to handle business logic.
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
        [FromServices] ITrackService trackService, 
        CancellationToken cancellationToken)
    {
        return await trackService.Delete(id, cancellationToken)
            ? Ok() 
            : NotFound();
    }
}