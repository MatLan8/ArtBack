namespace ArtBack.Domain.Entities;

public class Artwork: Entity
{
    public bool isDeleted { get; set; } = false;
    public required string Name { get; set; }
    public required string Author { get; set; }
    public required string Description { get; set; }
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required decimal Price { get; set; }
    public required string Dimensions { get; set; }
    public required string ImageUrl { get; set; }
    
    public required Guid VendorId { get; set; }
    public Guid CategoryId { get; set; }
    
    public Vendor? Vendor { get; set; }
    public Category? Category { get; set; }
    
    public ICollection<CartArtwork>? CartArtwork { get; set; }
    public ICollection<LikedArtwork>? LikedArtworks { get; set; }
    public ICollection<OrderArtwork>? OrderArtwork { get; set; }
}