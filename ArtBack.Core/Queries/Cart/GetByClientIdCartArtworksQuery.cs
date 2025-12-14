using ArtBack.Domain.Dtos;
using MediatR;

namespace ArtBack.Core.Queries.Cart;

public class GetByClientIdCartArtworksQuery : IRequest<List<CartArtworkDto>>
{
    public required Guid ClientId { get; set; }
}