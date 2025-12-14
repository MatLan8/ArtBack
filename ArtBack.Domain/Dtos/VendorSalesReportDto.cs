namespace ArtBack.Domain.Dtos;

public class VendorSalesReportDto
{
    public required decimal TotalRevenue { get; set; }
    public required int TotalSoldArtworks { get; set; }
    public required decimal AveragePrice { get; set; }
    public required HighestPricedArtDto HighestPricedArt { get; set; }
    public required MostPopularArtDto MostPopularArt { get; set; }
    public required List<PriceDistributionDto> PriceDistribution { get; set; }
    public required List<MonthlySalesDataDto> MonthlySalesData { get; set; }
    public required List<CategoryPerformanceDto> CategoryPerformance { get; set; }
    public required List<RecentOrderDto> RecentOrders { get; set; }
}

public class HighestPricedArtDto
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required int SoldCount { get; set; }
}

public class MostPopularArtDto
{
    public required string Name { get; set; }
    public required int SoldCount { get; set; }
    public required decimal Revenue { get; set; }
}

public class PriceDistributionDto
{
    public required string Range { get; set; }
    public required int Count { get; set; }
    public required decimal Revenue { get; set; }
}

public class MonthlySalesDataDto
{
    public required string Month { get; set; }
    public required decimal Revenue { get; set; }
    public required int SoldCount { get; set; }
}

public class CategoryPerformanceDto
{
    public required string CategoryName { get; set; }
    public required int SoldCount { get; set; }
    public required decimal Revenue { get; set; }
}

public class RecentOrderDto
{
    public required Guid OrderId { get; set; }
    public required string BuyerName { get; set; }
    public required string ArtworkName { get; set; }
    public required decimal Price { get; set; }
    public required DateTime SoldDate { get; set; }
    public required int Quantity { get; set; }
}
