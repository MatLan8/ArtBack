using ArtBack.Core.Commands.Client;
using ArtBack.Core.Queries.Client;
using ArtBack.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using ArtBack.Domain.Dtos;

namespace ArtBack.Api.Controllers;

[ApiController]
[Route("api/client")]
public class ClientController : BaseController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClient(Guid id)
        => Ok(await Mediator.Send(new GetClientByIdQuery { ClientId = id }));

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(Guid id, ClientDto dto)
    {
        await Mediator.Send(new UpdateClientCommand { ClientId = id, Client = dto });
        return NoContent();
    }

    [HttpGet("{id}/liked-artworks")]
    public async Task<IActionResult> GetLikedArtworks(Guid id)
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