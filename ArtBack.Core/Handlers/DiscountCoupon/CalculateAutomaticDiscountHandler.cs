using ArtBack.Core.Queries.DiscountCoupon;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.DiscountCoupon;

public class CalculateAutomaticDiscountHandler(ArtDbContext dbContext)
    : IRequestHandler<CalculateAutomaticDiscountQuery, decimal>
{

    public async Task<decimal> Handle(
        CalculateAutomaticDiscountQuery request,
        CancellationToken cancellationToken)
    {
        decimal discount = 0;
        
        if (request.CartTotal >= 100)
        {
            discount += 10;
        }
        
        var ordersCount = await dbContext.Orders
            .CountAsync(o => o.ClientId == request.ClientId, cancellationToken);

        if (ordersCount >= 5)
        {
            discount += request.CartTotal * 0.05m;
        }

        return discount;
    }
}