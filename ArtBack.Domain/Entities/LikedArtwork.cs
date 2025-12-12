namespace ArtBack.Domain.Entities;

public class LikedArtwork: Entity
{
    public bool isDeleted { get; set; } = false;
    
    public required Guid ClientId { get; set; }
    public required Guid ArtworkId { get; set; }
    
    public Client? Client { get; set; }
    public Artwork? Artwork { get; set; }
}