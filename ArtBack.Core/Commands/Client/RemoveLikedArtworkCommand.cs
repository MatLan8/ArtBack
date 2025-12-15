using MediatR;

namespace ArtBack.Core.Commands.Client;

public class RemoveLikedArtworkCommand : IRequest
{
    public Guid ClientId { get; set; }
    public Guid ArtworkId { get; set; }
}