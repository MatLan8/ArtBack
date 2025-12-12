using ArtBack.Core.Queries.Artwork;
using ArtBack.Domain.Dtos;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Artwork;


public class GetByIdArtworkQueryHandler(ArtDbContext dbContext) : IRequestHandler<GetByIdArtworkQuery, ArtworkDto>
{
    public async Task<ArtworkDto> Handle(GetByIdArtworkQuery request, CancellationToken cancellationToken)
    {
        var artwork = await dbContext.Artworks
            .AsNoTracking()
            .Where(a => !a.isDeleted && a.Id == request.Id)
            .Select(a => new ArtworkDto
            {
                Id = a.Id,
                Name = a.Name,
                Author = a.Author,
                Description = a.Description,
                Price = a.Price,
                Dimensions = a.Dimensions,
                ImageUrl = a.ImageUrl,

                Style = a.Category.Style,
                Material = a.Category.Material,
                Technique = a.Category.Technique,
                ColorPalette = a.Category.ColorPalette,
                ArtType = a.Category.ArtType,
                Period = a.Category.Period
            })
            .SingleOrDefaultAsync(cancellationToken);

        return artwork
               ?? throw new KeyNotFoundException($"Artwork {request.Id} not found");

    }
}