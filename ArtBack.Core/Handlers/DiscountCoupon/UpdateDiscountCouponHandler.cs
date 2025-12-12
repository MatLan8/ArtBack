using ArtBack.Core.Commands.DiscountCoupon;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.DiscountCoupon;

public class UpdateDiscountCouponHandler
    : IRequestHandler<UpdateDiscountCouponCommand, bool>
{
    private readonly ArtDbContext _context;

    public UpdateDiscountCouponHandler(ArtDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        UpdateDiscountCouponCommand request,
        CancellationToken cancellationToken)
    {
        var coupon = await _context.DiscountCoupons
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (coupon == null)
            return false;

        coupon.Description = request.Description;
        coupon.DiscountAmount = request.DiscountAmount;
        coupon.StartingPrice = request.StartingPrice;
        coupon.BeginAt = request.BeginAt;
        coupon.ExpireAt = request.ExpireAt;
        coupon.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}