using ArtBack.Domain.Entities;
using MediatR;

namespace ArtBack.Core.Queries.Client;

public class GetLikedArtworksQuery : IRequest<List<LikedArtwork>>
{
    public Guid ClientId { get; set; }
}