using ArtBack.Domain.Dtos;
using MediatR;

namespace ArtBack.Core.Queries.Artwork;

public class GetAllArtworksQuery : IRequest<List<ArtworkDto>>;