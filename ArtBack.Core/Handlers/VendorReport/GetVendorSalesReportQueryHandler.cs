using ArtBack.Core.Queries.VendorReport;
using ArtBack.Domain.Dtos;
using ArtBack.Domain.Entities;
using ArtBack.Domain.Types;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.VendorReport;

public class GetVendorSalesReportQueryHandler(ArtDbContext dbContext) : IRequestHandler<GetVendorSalesReportQuery, VendorSalesReportDto>
{
    public async Task<VendorSalesReportDto> Handle(GetVendorSalesReportQuery request, CancellationToken cancellationToken)
    {
        var vendorId = request.VendorId;

        // Basic metrics
        var basicMetrics = await dbContext.OrderArtworks
            .Where(oa => oa.Artwork!.VendorId == vendorId && oa.TotalSum > 0)
            .GroupBy(x => 1)
            .Select(g => new
            {
                TotalRevenue = (decimal?)g.Sum(oa => oa.TotalSum) ?? 0,
                TotalSoldArtworks = g.Count(),
                AveragePrice = (decimal?)g.Average(oa => oa.Artwork!.Price) ?? 0
            })
            .FirstOrDefaultAsync(cancellationToken) ?? new { TotalRevenue = 0m, TotalSoldArtworks = 0, AveragePrice = 0m };

        // Highest priced artwork with sales
        var highestPricedArt = await dbContext.Artworks
            .Where(a => a.VendorId == vendorId)
            .OrderByDescending(a => a.Price)
            .Select(a => new HighestPricedArtDto
            {
                Name = a.Name,
                Price = a.Price,
                SoldCount = a.OrderArtwork != null ? a.OrderArtwork.Count : 0
            })
            .FirstOrDefaultAsync(cancellationToken) ?? new HighestPricedArtDto { Name = "", Price = 0, SoldCount = 0 };

        // Most popular artwork
        var mostPopularArt = await dbContext.Artworks
            .Where(a => a.VendorId == vendorId)
            .Select(a => new
            {
                a.Name,
                SoldCount = a.OrderArtwork != null ? a.OrderArtwork.Count : 0,
                Revenue = a.OrderArtwork != null ? a.OrderArtwork.Sum(oa => oa.TotalSum) : 0m
            })
            .ToListAsync(cancellationToken);

        var mostPopularArtData = mostPopularArt
            .OrderByDescending(x => x.SoldCount)
            .FirstOrDefault() ?? new { Name = "", SoldCount = 0, Revenue = 0m };

        var mostPopularArtDto = new MostPopularArtDto
        {
            Name = mostPopularArtData.Name,
            SoldCount = mostPopularArtData.SoldCount,
            Revenue = mostPopularArtData.Revenue
        };

        // Price distribution
        var priceDistribution = await dbContext.OrderArtworks
            .Where(oa => oa.Artwork!.VendorId == vendorId)
            .GroupBy(oa => oa.Artwork!.Price)
            .Select(g => new
            {
                Price = g.Key,
                Count = g.Count(),
                Revenue = g.Sum(oa => oa.TotalSum)
            })
            .ToListAsync(cancellationToken);

        var priceDistributionDto = priceDistribution
            .GroupBy(x => GetPriceRange(x.Price))
            .Select(g => new PriceDistributionDto
            {
                Range = g.Key,
                Count = g.Sum(x => x.Count),
                Revenue = g.Sum(x => x.Revenue)
            })
            .OrderBy(x => GetRangeOrder(x.Range))
            .ToList();

        // Monthly sales data
        var monthlySalesData = await dbContext.Orders
            .Where(o => o.OrderArtwork!.Any(oa => oa.Artwork!.VendorId == vendorId) && o.TotalSum > 0)
            .ToListAsync(cancellationToken);

        var monthlySalesDataDto = monthlySalesData
            .GroupBy(o => new { Year = o.CreatedAt.Year, Month = o.CreatedAt.Month })
            .Select(g => new MonthlySalesDataDto
            {
                Month = $"{g.Key.Year:D4}-{g.Key.Month:D2}",
                Revenue = (decimal?)g.Sum(o => o.TotalSum) ?? 0,
                SoldCount = (int?)g.SelectMany(o => o.OrderArtwork!).Count() ?? 0
            })
            .OrderBy(x => x.Month)
            .ToList();

        // Category performance
        var categoryPerformance = await dbContext.Artworks
            .Where(a => a.VendorId == vendorId)
            .GroupBy(a => a.Category!.Style)
            .Select(g => new CategoryPerformanceDto
            {
                CategoryName = g.Key.ToString(),
                SoldCount = g.SelectMany(a => a.OrderArtwork!).Count(),
                Revenue = g.SelectMany(a => a.OrderArtwork!).Sum(oa => oa.TotalSum)
            })
            .OrderByDescending(x => x.Revenue)
            .ToListAsync(cancellationToken);

        // Recent orders (top 20)
        var recentOrders = await dbContext.Orders
            .Where(o => o.OrderArtwork!.Any(oa => oa.Artwork!.VendorId == vendorId))
            .OrderByDescending(o => o.CreatedAt)
            .Take(20)
            .ToListAsync(cancellationToken);

        var recentOrdersData = recentOrders
            .SelectMany(o => o.OrderArtwork!.Where(oa => oa.Artwork != null && oa.Artwork.VendorId == vendorId).Select(oa => new
            {
                o.Id,
                BuyerName = ((Domain.Entities.Client)o.Client!).FirstName + " " + ((Domain.Entities.Client)o.Client!).LastName,
                ArtworkName = oa.Artwork!.Name,
                oa.Artwork!.Price,
                o.CreatedAt,
                oa.ArtworkCount
            }))
            .ToList();

        var recentOrdersDto = recentOrdersData
            .Select(r => new RecentOrderDto
            {
                OrderId = r.Id,
                BuyerName = r.BuyerName,
                ArtworkName = r.ArtworkName,
                Price = r.Price,
                SoldDate = r.CreatedAt,
                Quantity = r.ArtworkCount
            })
            .ToList();

        return new VendorSalesReportDto
        {
            TotalRevenue = basicMetrics.TotalRevenue,
            TotalSoldArtworks = basicMetrics.TotalSoldArtworks,
            AveragePrice = basicMetrics.AveragePrice,
            HighestPricedArt = highestPricedArt,
            MostPopularArt = mostPopularArtDto,
            PriceDistribution = priceDistributionDto,
            MonthlySalesData = monthlySalesDataDto,
            CategoryPerformance = categoryPerformance,
            RecentOrders = recentOrdersDto
        };
    }

    private string GetPriceRange(decimal price)
    {
        return price switch
        {
            < 100 => "$0-$100",
            < 500 => "$100-$500",
            < 1000 => "$500-$1k",
            < 5000 => "$1k-$5k",
            _ => "$5k+"
        };
    }

    private int GetRangeOrder(string range)
    {
        return range switch
        {
            "$0-$100" => 1,
            "$100-$500" => 2,
            "$500-$1k" => 3,
            "$1k-$5k" => 4,
            "$5k+" => 5,
            _ => 6
        };
    }
}
