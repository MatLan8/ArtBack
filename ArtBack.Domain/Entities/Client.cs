namespace ArtBack.Domain.Entities;

public class Client: Entity
{
    public required int OrderedArtworkCount { get; set; }
    public ICollection<Order>? Orders { get; set; }
    public ICollection<LikedArtwork>? LikedArtworks { get; set; }
    public Cart? Cart { get; set; }
}