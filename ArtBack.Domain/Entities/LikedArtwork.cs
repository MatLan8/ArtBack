namespace ArtBack.Domain.Entities;

public class LikedArtwork: Entity
{
    public bool isDeleted { get; set; } = false;
    
    public required Guid userId { get; set; }
    public required Guid artworkId { get; set; }
    
    public User? User { get; set; }
    public Artwork? Artwork { get; set; }
}