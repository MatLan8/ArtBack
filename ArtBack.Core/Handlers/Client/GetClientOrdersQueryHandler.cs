using ArtBack.Core.Queries.Client;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ArtBack.Infrastructure;
using ArtBack.Domain.Entities;
using OrderEntity = ArtBack.Domain.Entities.Order;

public class GetClientOrdersQueryHandler
    : IRequestHandler<GetClientOrdersQuery, List<OrderEntity>>
{
    private readonly ArtDbContext _context;

    public GetClientOrdersQueryHandler(ArtDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderEntity>> Handle(
        GetClientOrdersQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Where(o => o.ClientId == request.ClientId)
            .Include(o => o.OrderArtwork)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}