using ArtBack.Core.Commands.Artwork;
using ArtBack.Core.Commands.Order;
using Microsoft.AspNetCore.Mvc;

namespace ArtBack.Api.Controllers;

public class OrderController : BaseController
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateOrderCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
}