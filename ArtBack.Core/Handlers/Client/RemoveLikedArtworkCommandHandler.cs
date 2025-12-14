using ArtBack.Core.Commands.Client;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Client;

public class RemoveLikedArtworkCommandHandler 
    : IRequestHandler<RemoveLikedArtworkCommand>
{
    private readonly ArtDbContext _context;

    public RemoveLikedArtworkCommandHandler(ArtDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        RemoveLikedArtworkCommand request,
        CancellationToken cancellationToken)
    {
        var likedArtwork = await _context.LikedArtworks
            .FirstOrDefaultAsync(
                x => x.ClientId == request.ClientId &&
                     x.ArtworkId == request.ArtworkId,
                cancellationToken);

        if (likedArtwork == null)
            throw new InvalidOperationException("Liked artwork not found");

        _context.LikedArtworks.Remove(likedArtwork);
        await _context.SaveChangesAsync(cancellationToken);
    }
}