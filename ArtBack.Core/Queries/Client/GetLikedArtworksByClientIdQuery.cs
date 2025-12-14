using ArtBack.Domain.Dtos;
using MediatR;

namespace ArtBack.Core.Queries.Client;

public class GetLikedArtworksByClientIdQuery: IRequest<List<ArtworkDto>>
{
    public required Guid ClientId { get; set; } 
}