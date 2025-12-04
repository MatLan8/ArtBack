namespace ArtBack.Domain.Entities;

public class LikedArtwork: Entity
{
    public Guid userId { get; set; }
    public Guid artworkId { get; set; }
    public User User { get; set; }
    public Artwork Artwork { get; set; }
}