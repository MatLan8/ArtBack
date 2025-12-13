using ArtBack.Core.Commands.DiscountCoupon;
using ArtBack.Infrastructure;
using MediatR;

namespace ArtBack.Core.Handlers.DiscountCoupon;

public class CreateDiscountCouponHandler(ArtDbContext dbContext)
    : IRequestHandler<CreateDiscountCouponCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateDiscountCouponCommand request,
        CancellationToken cancellationToken)
    {
        var coupon = new Domain.Entities.DiscountCoupon
        {
            CouponCode = request.CouponCode,
            Description = request.Description,
            DiscountAmount = request.DiscountAmount,
            StartingPrice = request.StartingPrice,
            BeginAt = request.BeginAt,
            ExpireAt = request.ExpireAt,
            IsActive = true
        };
        if (request.ExpireAt < request.BeginAt)
        {
            throw new ArgumentException("ExpireAt must be greater than BeginAt");
        }

        if (request.DiscountAmount < 0)
        {
            throw new ArgumentException("Discount amount must be greater than 0");
        }

        dbContext.DiscountCoupons.Add(coupon);
        await dbContext.SaveChangesAsync(cancellationToken);
        

        return coupon.Id;
    }
}