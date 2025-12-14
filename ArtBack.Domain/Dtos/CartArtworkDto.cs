namespace ArtBack.Domain.Dtos;

public class CartArtworkDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Author { get; set; }
    public required string ImageUrl { get; set; }
    public required decimal TotalSum { get; set; }
    public required string Dimensions { get; set; }
    public required int Count { get; set; }
    
}