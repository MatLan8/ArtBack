using MediatR;

namespace ArtBack.Core.Queries.Artwork;

public class GetAllArtworksQuery : IRequest<List<Domain.Entities.Artwork>>;