
using ArtBack.Core.Commands.Client;
using ArtBack.Core.Queries.Client;
using ArtBack.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ArtBack.Api.Controllers;

public class ClientController: BaseController
{
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClient(Guid id)
    {
        var result = await Mediator.Send(new GetClientByIdQuery
        {
            ClientId = id
        });

        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(Guid id, [FromBody] ClientDto dto)
    {
        await Mediator.Send(new UpdateClientCommand
        {
            ClientId = id,
            Client = dto
        });

        return NoContent();
    }
    
    
    [HttpPost("AddLikedArtwork")]
    public async Task<IActionResult> AddLikedArtwork(AddLikedArtworkCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            return Ok(result); 
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // "Artwork already liked"
        }
    }
    
    [HttpGet("{id}/liked-artworks")]
    public async Task<IActionResult> GetLikedArtworks(Guid id)
    {
        var result = await Mediator.Send(new GetLikedArtworksQuery
        {
            ClientId = id
        });

        return Ok(result);
    }
    
    [HttpDelete("{clientId}/liked-artworks/{artworkId}")]
    public async Task<IActionResult> RemoveLikedArtwork(Guid clientId, Guid artworkId)
    {
        await Mediator.Send(new RemoveLikedArtworkCommand
        {
            ClientId = clientId,
            ArtworkId = artworkId
        });

        return NoContent();
    }

}