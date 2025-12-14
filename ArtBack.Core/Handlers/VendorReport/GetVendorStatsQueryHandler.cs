using ArtBack.Core.Queries.VendorReport;
using ArtBack.Domain.Dtos;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.VendorReport;

public class GetVendorStatsQueryHandler(ArtDbContext dbContext) : IRequestHandler<GetVendorStatsQuery, VendorStatsDto>
{
    public async Task<VendorStatsDto> Handle(GetVendorStatsQuery request, CancellationToken cancellationToken)
    {
        var vendorId = request.VendorId;

        var vendor = await dbContext.Vendors.FirstOrDefaultAsync(v => v.Id == vendorId, cancellationToken);

        if (vendor == null)
        {
            throw new InvalidOperationException($"Vendor with ID {vendorId} not found");
        }

        var stats = await dbContext.Artworks
            .Where(a => a.VendorId == vendorId && !a.isDeleted)
            .GroupBy(x => 1)
            .Select(g => new
            {
                TotalArtworksCreated = g.Count(),
                TotalArtworksSold = g.SelectMany(a => a.OrderArtwork!).Count(),
                TotalRevenue = g.SelectMany(a => a.OrderArtwork!).Sum(oa => oa.TotalSum),
                AveragePrice = g.Average(a => a.Price)
            })
            .FirstOrDefaultAsync(cancellationToken) ?? new { TotalArtworksCreated = 0, TotalArtworksSold = 0, TotalRevenue = 0m, AveragePrice = 0m };

        return new VendorStatsDto
        {
            VendorId = vendorId,
            TotalArtworksCreated = stats.TotalArtworksCreated,
            TotalArtworksSold = stats.TotalArtworksSold,
            TotalRevenue = stats.TotalRevenue,
            AveragePrice = stats.AveragePrice
        };
    }
}
