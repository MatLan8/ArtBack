using ArtBack.Core.Commands.Payment;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;


namespace ArtBack.Core.Handlers.Payment;



public class CreateCheckoutSessionCommandHandler(ArtDbContext dbContext) : IRequestHandler<CreateCheckoutSessionCommand, string>
{
    public async Task<string> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
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

        int couponDiscountPercentage = 0;
        
        if (request.CouponId != null)
        {
            var discountCoupon = await dbContext.DiscountCoupons.FirstOrDefaultAsync(c => c.Id == request.CouponId, cancellationToken);
            
            if (discountCoupon != null)
            {
                couponDiscountPercentage = (int)discountCoupon.DiscountAmount;
            }
        }
        
        var couponDiscount = cart.TotalSum * couponDiscountPercentage/100m;
        
        var totalSum = cart.TotalSum - automaticDiscount - couponDiscount;

        if (totalSum <= 0)
        {
            throw new Exception("Invalid cart total after discounts");
        }
        
        long amountInCents = (long)Math.Round(totalSum * 100, MidpointRounding.AwayFromZero);

        var options = new SessionCreateOptions
        {
            Mode = "payment",
            PaymentMethodTypes = new List<string> { "card" },
            
            Metadata = new Dictionary<string, string>
            {
                { "orderId", request.OrderId.ToString() }
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "eur",
                        UnitAmount = amountInCents,
                        ProductData =
                            new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "ArtBack Order"
                            }
                    },
                    Quantity = 1
                }
            },
            SuccessUrl =
                "http://localhost:5173/payment-success?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = "http://localhost:5173/cart"
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Url;
    }
}