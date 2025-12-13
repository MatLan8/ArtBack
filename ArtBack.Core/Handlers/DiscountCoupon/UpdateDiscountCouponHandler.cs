using ArtBack.Core.Commands.DiscountCoupon;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.DiscountCoupon;

public class UpdateDiscountCouponHandler(ArtDbContext dbcontext)
    : IRequestHandler<UpdateDiscountCouponCommand, Unit>
{
    public async Task<Unit> Handle(
        UpdateDiscountCouponCommand request,
        CancellationToken cancellationToken)
    {
        var coupon = await dbcontext.DiscountCoupons
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (coupon == null)
            throw new Exception($"DiscountCoupon with ID {request.Id} not found");

        if (!string.IsNullOrEmpty(request.Description))
            coupon.Description = request.Description;

        if (request.DiscountAmount is double discountAmount)
            coupon.DiscountAmount = discountAmount;

        if (request.BeginAt is DateTime beginAt)
            coupon.BeginAt = beginAt;

        if (request.ExpireAt is DateTime expireAt)
            coupon.ExpireAt = expireAt;

        if (request.StartingPrice is decimal startingPrice)
            coupon.StartingPrice = startingPrice;

        if (request.IsActive is bool isActive)
            coupon.IsActive = isActive;

        await dbcontext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}