namespace ArtBack.Domain.Entities;

public class Category : Entity
{
    public required string Style { get; set; }
    public required string Material { get; set; }
    public required string Technique { get; set; }
    public required string ColorPalette { get; set; }
    public required string ArtType { get; set; }
    public required string Period { get; set; }
    public ICollection<Artwork>? Artworks { get; set; }
}