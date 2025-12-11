using ArtBack.Domain.Types;

namespace ArtBack.Domain.Entities;

public class Category : Entity
{
    public required Style Style { get; set; }
    public required Material Material { get; set; }
    public required Technique Technique { get; set; }
    public required ColorPallete ColorPalette { get; set; }
    public required ArtType ArtType { get; set; }
    public required Period Period { get; set; }
    
    
    public Artwork? Artwork { get; set; }
}