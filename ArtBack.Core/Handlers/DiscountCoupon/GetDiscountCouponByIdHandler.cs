using ArtBack.Core.Queries.DiscountCoupon;
using ArtBack.Domain.Dtos;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.DiscountCoupon;

public class GetDiscountCouponByIdHandler
    : IRequestHandler<GetByIdDiscountCouponQuery, DiscountCouponDto?>
{
    private readonly ArtDbContext _context;

    public GetDiscountCouponByIdHandler(ArtDbContext context)
    {
        _context = context;
    }

    public async Task<DiscountCouponDto?> Handle(
        GetByIdDiscountCouponQuery request,
        CancellationToken cancellationToken)
    {
        return await Queryable.Select(
                _context.DiscountCoupons.AsNoTracking(),
                c => new DiscountCouponDto
                {
                    Id = c.Id,
                    CouponCode = c.CouponCode,
                    Description = c.Description,
                    DiscountAmount = c.DiscountAmount,
                    StartingPrice = c.StartingPrice,
                    BeginAt = c.BeginAt,
                    ExpireAt = c.ExpireAt,
                    IsActive = c.IsActive
                })
            .FirstOrDefaultAsync(cancellationToken);




    }
}