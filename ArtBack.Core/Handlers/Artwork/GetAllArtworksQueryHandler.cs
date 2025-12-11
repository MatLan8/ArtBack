using ArtBack.Core.Queries.Artwork;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Artwork;

public class GetAllArtworksQueryHandler(ArtDbContext dbContext) : IRequestHandler<GetAllArtworksQuery, List<Domain.Entities.Artwork>>
{
    public async Task<List<Domain.Entities.Artwork>> Handle(GetAllArtworksQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Artworks.ToListAsync(cancellationToken);
    }
}