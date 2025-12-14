using ArtBack.Core.Queries.Cart;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Artwork;


public class GetAutomaticCartDiscountQueryHandler(ArtDbContext dbContext) : IRequestHandler<GetAutomaticCartDiscountQuery, double>
{
    public async Task<double> Handle(GetAutomaticCartDiscountQuery request, CancellationToken cancellationToken)
    {
        
        var cart = await dbContext.Carts
                       .FirstOrDefaultAsync(c => c.ClientId == request.ClientId, cancellationToken)
                   ?? throw new Exception("Cart not found");
        
        double discount = 0;

        if (cart.ArtworkCount > 3)
        {
            discount = 0.10;
        }

        if (cart.TotalSum > 25000)
        {
            discount += 0.05;
        }
        
        return discount;
    }
}