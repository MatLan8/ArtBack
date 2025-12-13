using ArtBack.Core.Commands.Artwork;
using ArtBack.Domain.Entities;
using ArtBack.Domain.Types;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Artwork;

public class AddCartArtworkCommandHandler(ArtDbContext dbContext)
    : IRequestHandler<AddCartArtworkCommand, Guid>
{
    public async Task<Guid> Handle(AddCartArtworkCommand request, CancellationToken cancellationToken)
    {   
        //ar turi cart
        var cart = await dbContext.Carts
            .FirstOrDefaultAsync(x => 
                x.ClientId == request.ClientId &&
                x.Status == Status.Active, 
                cancellationToken);
        //naujas cart
        if (cart == null)
        {
            cart = new Cart
            {
                ClientId = request.ClientId,
                Status = Status.Active,
                CreatedAt = DateTime.UtcNow,
                ArtworkCount = 0,
                TotalSum = 0
            };

            await dbContext.Carts.AddAsync(cart, cancellationToken);
        }

        // ar yra artwork cart
        var existingItem = await dbContext.CartArtworks
            .FirstOrDefaultAsync(x =>
                x.CartId == cart.Id &&
                x.ArtworkId == request.ArtworkId,
                cancellationToken);
        //yra
        if (existingItem != null)
        {
            existingItem.ArtworkCount += request.Count;
            existingItem.TotalSum += request.Price * request.Count;
            //summary
            cart.ArtworkCount += request.Count;
            cart.TotalSum += request.Price * request.Count;

            await dbContext.SaveChangesAsync(cancellationToken);

            return cart.Id;
        }

        // nėra - naujas
        var cartArtwork = new CartArtwork
        {
            CartId = cart.Id,
            ArtworkId = request.ArtworkId,
            ArtworkCount = request.Count,
            TotalSum = request.Price * request.Count,
            CreatedAt = DateTime.UtcNow
        };

        await dbContext.CartArtworks.AddAsync(cartArtwork, cancellationToken);
        //summarry
        cart.ArtworkCount += request.Count;
        cart.TotalSum += cartArtwork.TotalSum;

        await dbContext.SaveChangesAsync(cancellationToken);

        return cart.Id;
    }
}