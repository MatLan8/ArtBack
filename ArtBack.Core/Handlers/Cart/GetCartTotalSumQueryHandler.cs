using ArtBack.Core.Queries.Cart;
using ArtBack.Domain.Dtos;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Artwork;


public class GetCartTotalSumQueryHandler(ArtDbContext dbContext) : IRequestHandler<GetCartTotalSumQuery, decimal>
{
    public async Task<decimal> Handle(GetCartTotalSumQuery request, CancellationToken cancellationToken)
    {
        var totalSum = await dbContext.Carts
            .Where(c => c.ClientId == request.clientId)
            .Select(c => c.TotalSum)
            .SingleOrDefaultAsync(cancellationToken);

        return totalSum;
    }
}