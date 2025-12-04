namespace ArtBack.Domain.Entities;

public class Cart: Entity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required string Status { get; set; }
    public required int ArtworkCount { get; set; }
    public required double TotalSum { get; set; }
    public ICollection<CartArtwork>? CartArtwork { get; set; }
    public Client Client { get; set; }
    public Guid ClientId { get; set; }
}