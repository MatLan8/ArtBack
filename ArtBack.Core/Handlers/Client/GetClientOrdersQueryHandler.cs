using ArtBack.Core.Queries.Client;
using ArtBack.Domain.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ArtBack.Infrastructure;
using ArtBack.Domain.Entities;
using OrderEntity = ArtBack.Domain.Entities.Order;

public class GetClientOrdersQueryHandler
    : IRequestHandler<GetClientOrdersQuery, List<OrderDto>>
{
    private readonly ArtDbContext _context;

    public GetClientOrdersQueryHandler(ArtDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderDto>> Handle(
        GetClientOrdersQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Where(o => o.ClientId == request.ClientId)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                CreatedAt = o.CreatedAt,
                TotalSum = o.TotalSum,
                DeliveryStatus = o.DeliveryStatus
            })
            .ToListAsync(cancellationToken);
    }
}