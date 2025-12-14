using ArtBack.Core.Queries.VendorReport;
using ArtBack.Domain.Dtos;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.VendorReport;

public class GetVendorArtworksSalesDetailsQueryHandler(ArtDbContext dbContext) : IRequestHandler<GetVendorArtworksSalesDetailsQuery, List<ArtworksSalesDetailsDto>>
{
    public async Task<List<ArtworksSalesDetailsDto>> Handle(GetVendorArtworksSalesDetailsQuery request, CancellationToken cancellationToken)
    {
        var vendorId = request.VendorId;

        var artworksSalesDetails = await dbContext.Artworks
            .Where(a => a.VendorId == vendorId && !a.isDeleted)
            .Select(a => new ArtworksSalesDetailsDto
            {
                ArtworkId = a.Id,
                Name = a.Name,
                Price = a.Price,
                TotalSold = a.OrderArtwork != null ? a.OrderArtwork.Count : 0,
                TotalRevenue = a.OrderArtwork != null ? a.OrderArtwork.Sum(oa => oa.TotalSum) : 0m,
                Author = a.Author,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return artworksSalesDetails
            .OrderByDescending(x => x.TotalRevenue)
            .ToList();
    }
}