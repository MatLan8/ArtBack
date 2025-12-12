using ArtBack.Core.Queries.DiscountCoupon;
using ArtBack.Domain.Dtos;

namespace ArtBack.Core.Handlers.DiscountCoupon;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ArtBack.Infrastructure;

public class GetAllDiscountCouponsHandler
    : IRequestHandler<GetAllDiscountCouponQuery, List<DiscountCouponDto>>
{
    private readonly ArtDbContext _context;

    public GetAllDiscountCouponsHandler(ArtDbContext context)
    {
        _context = context;
    }

    public async Task<List<DiscountCouponDto>> Handle(
        GetAllDiscountCouponQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.DiscountCoupons
            .Select(c => new DiscountCouponDto
            {
                Id = c.Id,
                CouponCode = c.CouponCode,
                Description = c.Description,
                DiscountAmount = c.DiscountAmount,
                IsActive = c.IsActive,
                ExpireAt = c.ExpireAt
            })
            .ToListAsync(cancellationToken);
    }
}
