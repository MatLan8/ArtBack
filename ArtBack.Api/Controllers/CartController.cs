using ArtBack.Core.Commands.Artwork;
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
    
}