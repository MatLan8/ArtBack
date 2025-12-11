using ArtBack.Core.Commands.Artwork;
using ArtBack.Domain.Types;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Artwork;


public class UpdateArtworkCommandHandler(ArtDbContext dbcontext) : IRequestHandler<UpdateArtWorkCommand, Unit>
{
    public async Task<Unit> Handle(UpdateArtWorkCommand request, CancellationToken cancellationToken)
    {
        var artwork = await dbcontext.Artworks
            .FirstOrDefaultAsync(e => e.Id == request.ArtworkId, cancellationToken);

        if (artwork == null)
            throw new Exception($"Artwork with ID {request.ArtworkId} not found");

        if (!string.IsNullOrEmpty(request.Name))
            artwork.Name = request.Name;
        
        if (!string.IsNullOrEmpty(request.Author))
            artwork.Author = request.Author;
        
        if (!string.IsNullOrEmpty(request.Description))
            artwork.Description = request.Description;
        
        if (!string.IsNullOrEmpty(request.Dimensions))
            artwork.Dimensions = request.Dimensions;
        
        if (!string.IsNullOrEmpty(request.ImageUrl))
            artwork.ImageUrl = request.ImageUrl;
        
        if (request.Price is decimal price)
        {
            artwork.Price = price;
        }
        
        
        var category = await dbcontext.Categories
            .FirstOrDefaultAsync(c => c.Id == artwork.CategoryId, cancellationToken);

        if (category == null)
        {
            throw new Exception($"Category with ID {artwork.CategoryId} not found");
        }
        

        if (request.Style is Style style)
        {
            if (!Enum.IsDefined(typeof(Style), style))
                throw new Exception($"Invalid style value: {style}");

            category.Style = style;
        }
        
        if (request.Material is Material material)
        {
            if (!Enum.IsDefined(typeof(Material), material))
                throw new Exception($"Invalid Material value: {material}");

            category.Material = material;
        }

        if (request.Technique is Technique technique)
        {
            if (!Enum.IsDefined(typeof(Technique), technique))
                throw new Exception($"Invalid Technique value: {technique}");

            category.Technique = technique;
        }
        
        if (request.ColorPalette is ColorPallete colorPalette)
        {
            if (!Enum.IsDefined(typeof(ColorPallete), colorPalette))
                throw new Exception($"Invalid ColorPalette value: {colorPalette}");

            category.ColorPalette = colorPalette;
        }
        
        if (request.ArtType is ArtType artType)
        {
            if (!Enum.IsDefined(typeof(ArtType), artType))
                throw new Exception($"Invalid ArtType value: {artType}");

            category.ArtType = artType;
        }

        if (request.Period is Period period)
        {
            if (!Enum.IsDefined(typeof(Period), period))
                throw new Exception($"Invalid Period value: {period}");

            category.Period = period;
        }
        
        await dbcontext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}