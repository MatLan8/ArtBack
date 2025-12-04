namespace ArtBack.Domain.Entities;

public class Vendor: Entity
{
    public required int ArtworkCount { get; set; }
    public required int SoldArtworkCount { get; set; }
    public required double Profits { get; set; }
    public ICollection<Artwork>? Artworks { get; set; }
    
}