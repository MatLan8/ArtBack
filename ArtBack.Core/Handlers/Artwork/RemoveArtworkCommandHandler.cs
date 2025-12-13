using ArtBack.Core.Commands.Artwork;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Artwork;

public class RemoveArtworkCommandHandler(ArtDbContext dbContext) : IRequestHandler<RemoveArtworkCommand, bool>
{
    public async Task<bool> Handle(RemoveArtworkCommand request, CancellationToken cancellationToken)
    {
        var artwork = await dbContext.Artworks.FirstOrDefaultAsync(a => a.Id == request.ArtworkId, cancellationToken);

        if (artwork == null)
            throw new Exception($"Artwork with ID {request.ArtworkId} not found");

        artwork.isDeleted = true;

        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}