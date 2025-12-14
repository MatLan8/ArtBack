using ArtBack.Core.Commands.Order;
using ArtBack.Domain.Types;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Order;

public class CreateOrderCommandHandler(ArtDbContext dbContext) : IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        
        var cart = await dbContext.Carts.FirstOrDefaultAsync(c => c.ClientId == request.ClientId, cancellationToken);

        if (cart == null)
        {
            throw new Exception("Client cart not found");
        }
        
        decimal automaticDiscountPercentage = 0;

        if (cart.ArtworkCount > 3)
        {
            automaticDiscountPercentage = 0.10m;
        }

        if (cart.TotalSum > 25000)
        {
            automaticDiscountPercentage += 0.05m;
        }
        
        var automaticDiscount = cart.TotalSum * automaticDiscountPercentage;

        int couponDiscountPercantage = 0;
        Domain.Entities.DiscountCoupon? discountCoupon = null;

        if (request.DiscountCouponId != null)
        {
            discountCoupon = await dbContext.DiscountCoupons.FirstOrDefaultAsync(c => c.Id == request.DiscountCouponId, cancellationToken);
            
            if (discountCoupon != null)
            {
                couponDiscountPercantage = (int)discountCoupon.DiscountAmount;
            }
        }
        
        var couponDiscount = cart.TotalSum * couponDiscountPercantage/100m;
        
        var totalSum = cart.TotalSum - automaticDiscount - couponDiscount;
        
        
        var trackingId =  $"ART-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        
        

        var order = new Domain.Entities.Order
        {
            ClientId = request.ClientId,
            CreatedAt = DateTime.Now,
            TotalSum = totalSum,
            PaymentMethod = "Waiting for payment",
            DeliveryStatus = "Waiting for payment",
            DeliveryDate = DateTime.UtcNow.AddDays(14),
            TrackingNumber = trackingId,
            Comment = request.Comment,
            DeliveryMethod = request.DeliveryMethod,
            DeliveryAddress = request.DeliveryAddress,
            AppliedDiscount = (automaticDiscount + couponDiscount),
        };


        if (request.DiscountCouponId.HasValue)
        {
            order.DiscountCouponId = request.DiscountCouponId.Value;
            order.DiscountCoupon = discountCoupon;
        }
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}
