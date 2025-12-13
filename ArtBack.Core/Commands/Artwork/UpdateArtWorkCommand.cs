using ArtBack.Domain.Types;
using MediatR;

namespace ArtBack.Core.Commands.Artwork;

public class UpdateArtWorkCommand : IRequest<Unit>
{
    public required Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Author { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? Dimensions { get; set; }
    public string? ImageUrl { get; set; }
    
    public Style? Style { get; set; }
    public Material? Material { get; set; }
    public Technique? Technique { get; set; }
    public ColorPallete? ColorPalette { get; set; }
    public ArtType? ArtType { get; set; }
    public Period? Period { get; set; }
}