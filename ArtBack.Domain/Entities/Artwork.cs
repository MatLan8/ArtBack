namespace ArtBack.Domain.Entities;

public class Artwork: Entity
{
    public required string Name { get; set; }
    public required string Author { get; set; }
    public required string Description { get; set; }
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required double Price { get; set; }
    public required string Dimensions { get; set; }
    public required string ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public required Category Category { get; set; }
    public Guid VendorId { get; set; }
    public required Vendor Vendor { get; set; }
    public ICollection<CartArtwork>? CartArtwork { get; set; }
    public ICollection<LikedArtwork>? LikedArtworks { get; set; }
    public ICollection<OrderArtwork>? OrderArtwork { get; set; }
}