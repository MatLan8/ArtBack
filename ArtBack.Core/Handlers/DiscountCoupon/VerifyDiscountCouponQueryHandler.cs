using ArtBack.Core.Queries.Cart;
using ArtBack.Core.Queries.DiscountCoupon;
using ArtBack.Domain.Dtos;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.DiscountCoupon;


public class VerifyDiscountCouponQueryHandler(ArtDbContext dbContext) : IRequestHandler<VerifyDiscountCouponQuery, DiscountCouponVerificationDto>
{
    public async Task<DiscountCouponVerificationDto> Handle(VerifyDiscountCouponQuery request, CancellationToken cancellationToken)
    {
        var coupon = await dbContext.DiscountCoupons.FirstOrDefaultAsync(d => d.CouponCode == request.DiscountCode, cancellationToken);

        if (coupon == null)
        {
            return new DiscountCouponVerificationDto
            {
                IsValid = false,
                ErrorCode = "COUPON_NOT_FOUND",
                Message = "Coupon doesn't exist."
            };
        }

        if (!coupon.IsActive)
        {
            return new DiscountCouponVerificationDto
            {
                IsValid = false,
                ErrorCode = "COUPON_IS_NOT_ACTIVE",
                Message = "Coupon is not active."
            };
        }
        
        if (coupon.BeginAt > DateTime.Now)
        {
            return new DiscountCouponVerificationDto
            {
                IsValid = false,
                ErrorCode = "COUPON_HASNT_STARTED",
                Message = "Coupon has not been started yet."
            };
        }
        
        if (coupon.ExpireAt < DateTime.Now)
        {
            return new DiscountCouponVerificationDto
            {
                IsValid = false,
                ErrorCode = "COUPON_EXPIRED",
                Message = "Coupon has expired."
            };
        }
        
        var cart = await dbContext.Carts.FirstOrDefaultAsync(c => c.ClientId == request.ClientId, cancellationToken);

        if (cart == null)
        {
            throw new Exception("Client cart is not found.");
        }

        if (cart.TotalSum < coupon.StartingPrice)
        {
            return new DiscountCouponVerificationDto
            {
                IsValid = false,
                ErrorCode = "CART_DOES_NOT_QUALIFY",
                Message = "Your cart isn't eligible for this coupon."
            };
        }
        
        return new DiscountCouponVerificationDto
        {
            IsValid = true,
            DiscountValue = coupon.DiscountAmount/100,
            CouponId = coupon.Id,
        };
        
    }
}