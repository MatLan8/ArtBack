using ArtBack.Core.Commands.Artwork;
using ArtBack.Core.Queries.Cart;
using Microsoft.AspNetCore.Mvc;

namespace ArtBack.Api.Controllers;

public class CartController: BaseController
{
    
    [HttpPost("AddCartArtwork")]
    public async Task<IActionResult> AddCartArtwork(AddCartArtworkCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    
    [HttpGet("GetByClientIdCartArtworks")]
    public async Task<IActionResult> GetByClientIdCartArtworks([FromQuery] GetByClientIdCartArtworksQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    [HttpDelete("DeleteCartArtwork/{id:guid}")]
    public async Task<IActionResult> DeleteCartArtwork(Guid id)
    {
        var command = new DeleteCartArtworkCommand { CartArtworkId = id };
        await Mediator.Send(command);

        return Ok();
    }
    
    [HttpGet("GetCartTotalSum")]
    public async Task<IActionResult> GetCartTotalSum([FromQuery] GetCartTotalSumQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("GetAutomaticCartDiscount")]
    public async Task<IActionResult> GetAutomaticCartDiscount([FromQuery] GetAutomaticCartDiscountQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    
    
    
    
}