using MediatR;

namespace ArtBack.Core.Commands.Artwork;

public class DeleteAllCartArtworksCommand : IRequest<Unit>
{
    public required Guid clientId { get; set; }
}