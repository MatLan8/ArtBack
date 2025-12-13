using ArtBack.Core.Queries.DiscountCoupon;
using ArtBack.Domain.Dtos;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.DiscountCoupon;

public class GetDiscountCouponByIdHandler(ArtDbContext dbContext)
    : IRequestHandler<GetByIdDiscountCouponQuery, DiscountCouponDto>
{
    public async Task<DiscountCouponDto> Handle(
        GetByIdDiscountCouponQuery request,
        CancellationToken cancellationToken)
    {
        var coupon = await dbContext.DiscountCoupons
            .AsNoTracking()
            .Where(c => c.Id == request.Id)
            .Select(c => new DiscountCouponDto
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
            .SingleOrDefaultAsync(cancellationToken);

        return coupon
               ?? throw new KeyNotFoundException(
                   $"DiscountCoupon {request.Id} not found");
    }
}