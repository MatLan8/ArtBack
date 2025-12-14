using ArtBack.Core.Queries.Client;
using ArtBack.Domain.Dtos;
using ArtBack.Domain.Types;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Client;

public class GetLikedArtworksByClientIdHandler(ArtDbContext dbContext) 
    : IRequestHandler<GetLikedArtworksByClientIdQuery, List<ArtworkDto>>
{
    public async Task<List<ArtworkDto>> Handle(GetLikedArtworksByClientIdQuery request,
        CancellationToken cancellationToken)
    {
        var likedArtworks = await dbContext.LikedArtworks
            .Where(a => a.ClientId == request.ClientId && !a.isDeleted)
            .Include(a => a.Artwork) 
            .ThenInclude(a => a.Category)
            .ToListAsync(cancellationToken);

        // now project in-memory
        var dtos = likedArtworks
            .Where(a => a.Artwork != null)
            .Select(a => new ArtworkDto
            {
                Id = a.Artwork!.Id,
                Name = a.Artwork.Name,
                Author = a.Artwork.Author,
                Description = a.Artwork.Description,
                CreatedAt = a.Artwork.CreatedAt,
                Price = a.Artwork.Price,
                Dimensions = a.Artwork.Dimensions,
                ImageUrl = a.Artwork.ImageUrl,
                
                Style = a.Artwork.Category.Style,
                Material = a.Artwork.Category.Material,
                Technique = a.Artwork.Category.Technique,
                ColorPalette = a.Artwork.Category.ColorPalette,
                ArtType = a.Artwork.Category.ArtType,
                Period = a.Artwork.Category.Period,
            })
            .ToList();

        return dtos;
    }
}
