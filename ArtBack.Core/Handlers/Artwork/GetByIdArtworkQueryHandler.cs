using ArtBack.Core.Queries.Artwork;
using ArtBack.Infrastructure;
using MediatR;

namespace ArtBack.Core.Handlers.Artwork;


public class GetByIdArtworkQueryHandler(ArtDbContext dbContext) : IRequestHandler<GetByIdArtworkQuery, Domain.Entities.Artwork>
{
    public async Task<Domain.Entities.Artwork> Handle(GetByIdArtworkQuery request, CancellationToken cancellationToken)
    {
        var artwork = await dbContext.Artworks.FindAsync([request.Id], cancellationToken)
                      ?? throw new NullReferenceException();

        return artwork;

    }
}