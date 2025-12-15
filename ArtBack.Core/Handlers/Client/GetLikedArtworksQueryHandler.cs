using ArtBack.Core.Queries.Client;
using ArtBack.Domain.Entities;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Client;

public class GetLikedArtworksQueryHandler
    : IRequestHandler<GetLikedArtworksQuery, List<LikedArtwork>>
{
    private readonly ArtDbContext _db;

    public GetLikedArtworksQueryHandler(ArtDbContext db)
    {
        _db = db;
    }

    public async Task<List<LikedArtwork>> Handle(
        GetLikedArtworksQuery request,
        CancellationToken cancellationToken)
    {
        return await _db.LikedArtworks
            .AsNoTracking()
            .Where(x => x.ClientId == request.ClientId && !x.isDeleted)
            .ToListAsync(cancellationToken);
    }
}