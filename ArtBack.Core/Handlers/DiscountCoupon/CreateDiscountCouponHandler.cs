using ArtBack.Core.Commands.DiscountCoupon;
using ArtBack.Infrastructure;
using MediatR;

namespace ArtBack.Core.Handlers.DiscountCoupon;

public class CreateDiscountCouponHandler
    : IRequestHandler<CreateDiscountCouponCommand, Guid>
{
    private readonly ArtDbContext _context;

    public CreateDiscountCouponHandler(ArtDbContext context)
    {
        _context = context;
    }

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

        _context.DiscountCoupons.Add(coupon);
        await _context.SaveChangesAsync(cancellationToken);

        return coupon.Id;
    }
}