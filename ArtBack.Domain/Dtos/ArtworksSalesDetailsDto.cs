namespace ArtBack.Domain.Dtos;

public class ArtworksSalesDetailsDto
{
    public required Guid ArtworkId { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required int TotalSold { get; set; }
    public required decimal TotalRevenue { get; set; }
    public required string Author { get; set; }
    public required DateTime CreatedAt { get; set; }
}
