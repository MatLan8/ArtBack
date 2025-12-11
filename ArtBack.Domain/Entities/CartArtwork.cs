namespace ArtBack.Domain.Entities;

public class CartArtwork: Entity
{
    public bool isDeleted { get; set; } = false;
    
    public required int ArtworkCount { get; set; }
    public required decimal TotalSum { get; set; }
    public required DateTime CreatedAt { get; set; }
    
    public required Guid CartId { get; set; }
    public required Guid ArtworkId { get; set; }
    
    public Cart? Cart { get; set; }
    public Artwork? Artwork { get; set; }
}
