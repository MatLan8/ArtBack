using ArtBack.Domain.Dtos;
using ArtBack.Infrastructure;
using ArtBack.Core.Queries.DiscountCoupon;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.DiscountCoupon;

public class ValidateDiscountCouponHandler
    : IRequestHandler<ValidateDiscountCouponQuery, ValidateDiscountCouponDto?>
{
    private readonly ArtDbContext _context;

    public ValidateDiscountCouponHandler(ArtDbContext context)
    {
        _context = context;
    }

    public async Task<ValidateDiscountCouponDto?> Handle(
        ValidateDiscountCouponQuery request,
        CancellationToken cancellationToken)
    {
        var coupon = await _context.DiscountCoupons
            .FirstOrDefaultAsync(c =>
                    c.CouponCode == request.Code &&
                    c.IsActive &&
                    c.BeginAt <= DateTime.UtcNow &&
                    c.ExpireAt >= DateTime.UtcNow,
                cancellationToken);

        if (coupon == null)
            return null;

        if (request.CartTotal < coupon.StartingPrice)
            return null;

        return new ValidateDiscountCouponDto
        {
            CouponCode = coupon.CouponCode,
            DiscountAmount = coupon.DiscountAmount,
            StartingPrice = coupon.StartingPrice,
            ExpireAt = coupon.ExpireAt
        };
    }
}