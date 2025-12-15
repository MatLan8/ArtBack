using ArtBack.Core.Commands.Payment;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace ArtBack.Api.Controllers;

public class PaymentController : BaseController
{
    [HttpPost("CreateCheckoutSession")]
    public async Task<IActionResult> CreateCheckoutSession(
        CreateCheckoutSessionCommand command)
    {
        var url = await Mediator.Send(command);
        return Ok(url);
    }
    
    
    [HttpPost("ConfirmPayment")]
    public async Task<IActionResult> ConfirmPayment([FromBody] string sessionId)
    {
        var result = await Mediator.Send(
            new ConfirmStripePaymentCommand { SessionId = sessionId }
        );

        return Ok(result);
    }
}