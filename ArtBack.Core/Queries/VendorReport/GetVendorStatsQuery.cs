using ArtBack.Domain.Dtos;
using MediatR;

namespace ArtBack.Core.Queries.VendorReport;

public class GetVendorStatsQuery : IRequest<VendorStatsDto>
{
    public required Guid VendorId { get; set; }
}
