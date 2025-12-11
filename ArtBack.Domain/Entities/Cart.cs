using ArtBack.Domain.Types;

namespace ArtBack.Domain.Entities;

public class Cart: Entity
{
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required Status Status { get; set; }
    public required int ArtworkCount { get; set; }
    public required decimal TotalSum { get; set; }
    public required Guid ClientId { get; set; }
    
    
    public ICollection<CartArtwork>? CartArtwork { get; set; }
    public Client? Client { get; set; }

}