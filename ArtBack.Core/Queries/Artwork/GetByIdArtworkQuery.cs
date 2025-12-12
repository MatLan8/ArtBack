using ArtBack.Domain.Dtos;
using MediatR;

namespace ArtBack.Core.Queries.Artwork;

public class GetByIdArtworkQuery : IRequest<ArtworkDto>
{
    public required Guid Id { get; set; }
}