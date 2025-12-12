using ArtBack.Core.Commands.Artwork;
using Microsoft.AspNetCore.Mvc;

namespace ArtBack.Api.Controllers;

public class CartController: BaseController
{
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateOrAddArtworkToCartCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
}