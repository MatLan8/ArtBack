namespace ArtBack.Domain.Entities;

public class OrderArtwork: Entity
{
    public required double TotalSum { get; set; }
    public required int ArtworkCount { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    public Guid ArtworkId { get; set; }
    public Artwork Artwork { get; set; }
}