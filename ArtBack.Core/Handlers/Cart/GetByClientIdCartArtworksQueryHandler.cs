using ArtBack.Core.Queries.Artwork;
using ArtBack.Core.Queries.Cart;
using ArtBack.Domain.Dtos;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Artwork;



public class GetByClientIdCartArtworksQueryHandler(ArtDbContext dbContext) : IRequestHandler<GetByClientIdCartArtworksQuery, List<CartArtworkDto>>
{
    public async Task<List<CartArtworkDto>> Handle(GetByClientIdCartArtworksQuery request, CancellationToken cancellationToken)
    {
        var cartArtworks = await dbContext.CartArtworks
            .AsNoTracking()
            .Where(ca =>
                ca.Cart!.ClientId == request.ClientId &&
                !ca.isDeleted &&
                !ca.Artwork!.isDeleted)
            .Select(ca => new CartArtworkDto
            {
                Id = ca.Id,
                Name = ca.Artwork!.Name,
                Author = ca.Artwork.Author,
                ImageUrl = ca.Artwork.ImageUrl,
                Dimensions = ca.Artwork.Dimensions,
                TotalSum = ca.TotalSum,
                Count = ca.ArtworkCount
            })
            .ToListAsync(cancellationToken);

        return cartArtworks;
        
    }
}