using ArtBack.Core.Commands.DiscountCoupon;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.DiscountCoupon;

public class DeactivateDiscountCouponHandler
    : IRequestHandler<DeactivateDiscountCouponCommand, bool>
{
    private readonly ArtDbContext _context;

    public DeactivateDiscountCouponHandler(ArtDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        DeactivateDiscountCouponCommand request,
        CancellationToken cancellationToken)
    {
        var coupon = await _context.DiscountCoupons
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (coupon == null)
            return false;

        coupon.IsActive = false;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}