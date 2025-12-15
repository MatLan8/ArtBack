
using ArtBack.Core.Commands.Client;
using ArtBack.Core.Queries.Client;
using Microsoft.AspNetCore.Mvc;
using ArtBack.Domain.Dtos;

namespace ArtBack.Api.Controllers;

[ApiController]
[Route("api/client")]
public class ClientController : BaseController
{
    // ====== FIXED ROUTES (be {id}) ======

    [HttpGet("GetRecommendedArtworks")]
    public async Task<IActionResult> GetRecommendedArtworks([FromQuery]  GetRecommendedArtworksQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
        
    }

    [HttpPost("liked-artworks")]
    public async Task<IActionResult> AddLikedArtwork(
        [FromBody] AddLikedArtworkCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    // ====== ID BASED ROUTES ======

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClient(Guid id)
    {
        return Ok(await Mediator.Send(new GetClientByIdQuery
        {
            ClientId = id
        }));
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

    [HttpGet("{id}/liked-artworks")]
    public async Task<IActionResult> GetLikedArtworks(Guid id)
    {
        return Ok(await Mediator.Send(new GetLikedArtworksQuery
        {
            ClientId = id
        }));
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
        return Ok(await Mediator.Send(new GetClientOrdersQuery
        {
            ClientId = id
        }));
    }

    [HttpGet("GetLikedArtworks")]
    public async Task<IActionResult> GetLikedArtworks([FromQuery] GetLikedArtworksByClientIdQuery  query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
 
    
}
