namespace ArtBack.Domain.Entities;

public class Vendor: User
{
    public required int ArtworkCount { get; set; }
    public required int SoldArtworkCount { get; set; }
    public required decimal Profits { get; set; }
    
    public ICollection<Artwork>? Artworks { get; set; }
    
}