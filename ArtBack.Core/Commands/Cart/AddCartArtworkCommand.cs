using MediatR;

namespace ArtBack.Core.Commands.Artwork;

public class AddCartArtworkCommand : IRequest<Guid>
{
    public required Guid ClientId { get; set; }
    public required Guid ArtworkId { get; set; }
    public required int Count { get; set; }
    public required decimal Price { get; set; }
}