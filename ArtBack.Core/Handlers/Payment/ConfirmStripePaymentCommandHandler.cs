using ArtBack.Core.Commands.Payment;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace ArtBack.Core.Handlers.Payment;

public class ConfirmStripePaymentCommandHandler(ArtDbContext dbContext) : IRequestHandler<ConfirmStripePaymentCommand, bool>
{
    public async Task<bool> Handle(ConfirmStripePaymentCommand request, CancellationToken cancellationToken)
    {
        // 1️⃣ Get Stripe session
        var sessionService = new SessionService();
        var session = await sessionService.GetAsync(request.SessionId);

        if (session.PaymentStatus != "paid")
            throw new Exception("Payment not completed");

        // 2️⃣ Get payment intent
        var paymentIntentService = new PaymentIntentService();
        var paymentIntent = await paymentIntentService.GetAsync(
            session.PaymentIntentId
        );

        // 3️⃣ Extract orderId from metadata
        if (!session.Metadata.TryGetValue("orderId", out var orderIdRaw))
            throw new Exception("OrderId missing in Stripe metadata");

        var orderId = Guid.Parse(orderIdRaw);

        var order = await dbContext.Orders
                        .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken)
                    ?? throw new Exception("Order not found");

        // 4️⃣ Update order
        
        var paymentMethodService = new PaymentMethodService();
        var paymentMethod = await paymentMethodService.GetAsync(
            paymentIntent.PaymentMethodId
        );
        
        var card = paymentMethod.Card.Brand;
        
        order.PaymentMethod = card;
        order.DeliveryStatus = "Sent";

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}