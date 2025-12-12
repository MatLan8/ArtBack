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
        // 1. Patikrinti ar klientas turi aktyvų Cart
        var cart = await dbContext.Carts
            .FirstOrDefaultAsync(x => 
                x.ClientId == request.ClientId &&
                x.Status == Status.Active, 
                cancellationToken);

        // 2. Jei ne — sukurti naują Cart
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

        // 3. Patikrinti ar šis Artwork jau yra Cart’e
        var existingItem = await dbContext.CartArtworks
            .FirstOrDefaultAsync(x =>
                x.CartId == cart.Id &&
                x.ArtworkId == request.ArtworkId,
                cancellationToken);

        if (existingItem != null)
        {
            // 4. Jei yra – tiesiog padidinti kiekį
            existingItem.ArtworkCount += request.Count;
            existingItem.TotalSum += request.Price * request.Count;

            // Atnaujinti Cart totals
            cart.ArtworkCount += request.Count;
            cart.TotalSum += request.Price * request.Count;

            await dbContext.SaveChangesAsync(cancellationToken);

            return cart.Id;
        }

        // 5. Jei nėra – sukurti naują CartArtwork
        var cartArtwork = new CartArtwork
        {
            CartId = cart.Id,
            ArtworkId = request.ArtworkId,
            ArtworkCount = request.Count,
            TotalSum = request.Price * request.Count,
            CreatedAt = DateTime.UtcNow
        };

        await dbContext.CartArtworks.AddAsync(cartArtwork, cancellationToken);

        // 6. Atnaujinti Cart summary
        cart.ArtworkCount += request.Count;
        cart.TotalSum += cartArtwork.TotalSum;

        await dbContext.SaveChangesAsync(cancellationToken);

        return cart.Id;
    }
}