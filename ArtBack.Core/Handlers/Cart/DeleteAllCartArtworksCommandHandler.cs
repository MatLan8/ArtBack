using ArtBack.Core.Commands.Artwork;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Artwork;



public class DeleteAllCartArtworksCommandHandler(ArtDbContext context) : IRequestHandler<DeleteAllCartArtworksCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAllCartArtworksCommand request, CancellationToken cancellationToken)
    {
        

        var cart = await context.Carts
                       .FirstOrDefaultAsync(c => c.ClientId == request.clientId, cancellationToken)
                   ?? throw new Exception($"Cart with ID {request.clientId} not found");


        cart.ArtworkCount = 0;
        cart.TotalSum = 0;
        
        var cartId = cart.Id;

        
        await context.CartArtworks
            .Where(ca => ca.CartId == cartId)
            .ExecuteDeleteAsync(cancellationToken);
        
        

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}