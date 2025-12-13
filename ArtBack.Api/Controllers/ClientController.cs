
using ArtBack.Core.Commands.Client;
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
}