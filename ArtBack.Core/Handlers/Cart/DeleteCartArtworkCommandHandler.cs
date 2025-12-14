using ArtBack.Core.Commands.Artwork;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Artwork;


public class DeleteCartArtworkCommandHandler(ArtDbContext context) : IRequestHandler<DeleteCartArtworkCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCartArtworkCommand request, CancellationToken cancellationToken)
    {
        var cartArtwork = await context.CartArtworks
                              .Include(ca => ca.Artwork)
                              .FirstOrDefaultAsync(ca => ca.Id == request.CartArtworkId, cancellationToken)
                          ?? throw new Exception($"Cart artwork with ID {request.CartArtworkId} not found");

        var cart = await context.Carts
                       .FirstOrDefaultAsync(c => c.Id == cartArtwork.CartId, cancellationToken)
                   ?? throw new Exception($"Cart with ID {cartArtwork.CartId} not found");

        var price = cartArtwork.Artwork.Price;

        if (cartArtwork.ArtworkCount > 1)
        {
            cartArtwork.ArtworkCount--;
            cartArtwork.TotalSum -= price;
        }
        else
        {
            context.CartArtworks.Remove(cartArtwork);
        }

        cart.ArtworkCount = Math.Max(0, cart.ArtworkCount - 1);
        cart.TotalSum = Math.Max(0, cart.TotalSum - price);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}