using ArtBack.Core.Queries.Artwork;
using ArtBack.Domain.Dtos;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Artwork;

public class GetAllArtworksQueryHandler(ArtDbContext dbContext) : IRequestHandler<GetAllArtworksQuery, List<ArtworkDto>>
{
    public async Task<List<ArtworkDto>> Handle(GetAllArtworksQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Artworks
            .AsNoTracking()
            .Where(a => !a.isDeleted)
            .Select(a => new ArtworkDto
            {
                // Artwork properties
                Id = a.Id,
                Name = a.Name,
                Author = a.Author,
                Description = a.Description,
                CreatedAt = a.CreatedAt,
                Price = a.Price,
                Dimensions = a.Dimensions,
                ImageUrl = a.ImageUrl,

                // Category properties
                Style = a.Category.Style,
                Material = a.Category.Material,
                Technique = a.Category.Technique,
                ColorPalette = a.Category.ColorPalette,
                ArtType = a.Category.ArtType,
                Period = a.Category.Period
            })
            .ToListAsync(cancellationToken);
        
        
    }
}