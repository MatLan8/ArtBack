using MediatR;

namespace ArtBack.Core.Commands.Artwork;

public class RemoveArtworkCommand : IRequest<bool>
{
    public required Guid ArtworkId { get; set; } 
}