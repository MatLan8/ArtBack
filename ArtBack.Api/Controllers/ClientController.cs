
using ArtBack.Core.Commands.Client;
using ArtBack.Core.Queries.Client;
using ArtBack.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using ArtBack.Domain.Dtos;

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
    
 
    [HttpGet("GetRecommendedArtworks")]
    public async Task<IActionResult> GetRecommendedArtworks([FromQuery]  GetRecommendedArtworksQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
        
    }
}
