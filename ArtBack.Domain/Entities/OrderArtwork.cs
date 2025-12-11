namespace ArtBack.Domain.Entities;

public class OrderArtwork: Entity
{
    public required decimal TotalSum { get; set; }
    public required int ArtworkCount { get; set; }
    
    public required Guid OrderId { get; set; }
    public required Guid ArtworkId { get; set; }
    
    public Order? Order { get; set; }
    public Artwork? Artwork { get; set; }
}