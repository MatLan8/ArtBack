using ArtBack.Domain.Types;

namespace ArtBack.Domain.Dtos;

public class ArtworkDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Author { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required string Dimensions { get; set; }
    public required string ImageUrl { get; set; }
    public required Style Style { get; set; }
    public required Material Material { get; set; }
    public required Technique Technique { get; set; }
    public required ColorPallete ColorPalette { get; set; }
    public required ArtType ArtType { get; set; }
    public required Period Period { get; set; }
}