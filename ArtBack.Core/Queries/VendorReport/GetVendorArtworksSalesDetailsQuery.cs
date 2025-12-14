using ArtBack.Domain.Dtos;
using MediatR;

namespace ArtBack.Core.Queries.VendorReport;

public class GetVendorArtworksSalesDetailsQuery : IRequest<List<ArtworksSalesDetailsDto>>
{
    public required Guid VendorId { get; set; }
}
