using MediatR;
using System;
namespace ArtBack.Core.Commands.Client;

public class AddLikedArtworkCommand: IRequest<bool>
{
    public required Guid ClientId { get; set; }
    public required Guid ArtworkId { get; set; }
}