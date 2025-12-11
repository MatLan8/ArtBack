using MediatR;

namespace ArtBack.Core.Queries.Artwork;

public class GetByIdArtworkQuery : IRequest<Domain.Entities.Artwork>
{
    public required Guid Id { get; set; }
}