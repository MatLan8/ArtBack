
using ArtBack.Core.Commands.Client;
using ArtBack.Core.Queries.Client;
using ArtBack.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ArtBack.Api.Controllers;

public class ClientController: BaseController
{
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

    [HttpGet("GetLikedArtworks")]
    public async Task<IActionResult> GetLikedArtworks([FromQuery] GetLikedArtworksByClientIdQuery  query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("GetRecommendedArtworks")]
    public async Task<IActionResult> GetRecommendedArtworks([FromQuery]  GetRecommendedArtworksQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
        
    }
    
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClient(Guid id)
        => Ok(await Mediator.Send(new GetClientByIdQuery { ClientId = id }));

    [HttpPut("UpdateClient/{id}")]
    public async Task<IActionResult> UpdateClient(Guid id, ClientDto dto)
    {
        await Mediator.Send(new UpdateClientCommand
        {
            ClientId = id,
            Client = dto
        });

        return NoContent();
    }

    [HttpGet("{id}/liked-artworks")]
    public async Task<IActionResult> GetLikedArtworks1(Guid id)
        => Ok(await Mediator.Send(new GetLikedArtworksQuery { ClientId = id }));

    [HttpPost("{id}/liked-artworks")]
    public async Task<IActionResult> AddLikedArtwork(Guid id, AddLikedArtworkCommand command)
    {
        command.ClientId = id;
        return Ok(await Mediator.Send(command));
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

    [HttpGet("{id}/orders")]
    public async Task<IActionResult> GetOrders(Guid id)
    {
        return Ok(await Mediator.Send(
            new GetClientOrdersQuery { ClientId = id }
        ));
    }
    

}