using MediatR;

namespace ArtBack.Core.Commands.Payment;

public class ConfirmStripePaymentCommand : IRequest<bool>
{
    public required string SessionId { get; set; }
}