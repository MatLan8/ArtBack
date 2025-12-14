namespace ArtBack.Domain.Dtos;

public class VendorStatsDto
{
    public required Guid VendorId { get; set; }
    public required int TotalArtworksCreated { get; set; }
    public required int TotalArtworksSold { get; set; }
    public required decimal TotalRevenue { get; set; }
    public required decimal AveragePrice { get; set; }
}
