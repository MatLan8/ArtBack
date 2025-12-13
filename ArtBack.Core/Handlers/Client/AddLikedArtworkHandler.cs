using ArtBack.Core.Commands.Client;
using ArtBack.Domain.Entities;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ArtBack.Core.Handlers.Client;

public class AddLikedArtworkHandler(ArtDbContext dbContext)
    : IRequestHandler<AddLikedArtworkCommand, bool>
{
    public async Task<bool> Handle(AddLikedArtworkCommand request, CancellationToken cancellationToken)
    {   
        //ar liked
        var existingLike = await dbContext.LikedArtworks
            .FirstOrDefaultAsync(
                x => x.ClientId == request.ClientId
                     && x.ArtworkId == request.ArtworkId,
                cancellationToken
            );
        
        //taip
        if (existingLike != null)
            throw new InvalidOperationException("Artwork already liked");
        
        //ne
        var likedArtwork = new LikedArtwork
        {
            ClientId = request.ClientId,
            ArtworkId = request.ArtworkId,
            isDeleted = false
        };
        
        dbContext.LikedArtworks.AddAsync(likedArtwork, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

}