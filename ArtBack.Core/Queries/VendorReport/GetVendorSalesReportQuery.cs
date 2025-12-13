using ArtBack.Domain.Dtos;
using MediatR;

namespace ArtBack.Core.Queries.VendorReport;

public class GetVendorSalesReportQuery : IRequest<VendorSalesReportDto>
{
    public required Guid VendorId { get; set; }
}
