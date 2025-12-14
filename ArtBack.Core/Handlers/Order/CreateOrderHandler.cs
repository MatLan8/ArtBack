using ArtBack.Core.Commands.Order;
using ArtBack.Domain.Types;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Order;

public class CreateOrderHandler(ArtDbContext dbContext)
    : IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        decimal cartTotal = await dbContext.CartArtworks
            .Where(c => c.Cart!.ClientId == request.ClientId && !c.isDeleted)
            .SumAsync(c => c.TotalSum, cancellationToken);


        Domain.Entities.DiscountCoupon? coupon = null;
        decimal discount = 0m;

        if (!string.IsNullOrWhiteSpace(request.CouponCode))
        {
            coupon = await dbContext.DiscountCoupons.FirstOrDefaultAsync(c =>
                c.CouponCode == request.CouponCode &&
                c.IsActive &&
                c.BeginAt <= DateTime.UtcNow &&
                c.ExpireAt >= DateTime.UtcNow,
                cancellationToken);

            if (coupon == null)
                throw new Exception("Invalid or expired coupon");

            if (cartTotal < coupon.StartingPrice)
                throw new Exception("Cart total too low for this coupon");
            
            discount = (decimal)coupon.DiscountAmount;
        }

        var order = new Domain.Entities.Order
        {
            ClientId = request.ClientId,
            TotalSum = cartTotal - discount,
            AppliedDiscount = discount,
            DiscountCouponId = coupon?.Id,

            CreatedAt = DateTime.UtcNow,
            DeliveryStatus = "Pending",
            PaymentMethod = "Card",
            DeliveryAddress = "N/A",
            TrackingNumber = Guid.NewGuid().ToString(),
            Comment = "",
            DeliveryDate = DateTime.UtcNow.AddDays(3),
            DeliveryMethod = DeliveryMethod.Courier
        };

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}
