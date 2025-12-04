namespace ArtBack.Domain.Entities;

public class CartArtwork: Entity
{
    public required int ArtworkCount { get; set; }
    public required double TotalSum { get; set; }
    public DateTime CreatedAt { get; set; }
    public required Guid CartId { get; set; }
    public Cart Cart { get; set; }
    public required Guid ArtworkId { get; set; }
    public Artwork Artwork { get; set; }
}
