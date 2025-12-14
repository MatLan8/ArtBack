using MediatR;

namespace ArtBack.Core.Commands.Artwork;

public class DeleteCartArtworkCommand : IRequest<Unit>
{
    public required Guid CartArtworkId { get; set; }
}