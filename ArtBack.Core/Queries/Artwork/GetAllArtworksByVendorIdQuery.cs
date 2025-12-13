using ArtBack.Domain.Dtos;
using MediatR;

namespace ArtBack.Core.Queries.Artwork;

public class GetAllArtworksByVendorIdQuery : IRequest<List<ArtworkDto>>
{ 
    public required Guid VendorId { get; set; }
}