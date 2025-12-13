using ArtBack.Core.Commands.DiscountCoupon;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.DiscountCoupon;

public class DeactivateDiscountCouponHandler(ArtDbContext dbContext)
    : IRequestHandler<DeactivateDiscountCouponCommand, bool>
{

    public async Task<bool> Handle(
        DeactivateDiscountCouponCommand request,
        CancellationToken cancellationToken)
    {
        var coupon = await dbContext.DiscountCoupons
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (coupon == null)
            return false;

        coupon.IsActive = false;
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}