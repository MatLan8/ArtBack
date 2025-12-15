using MediatR;

namespace ArtBack.Core.Commands.Payment;

public class CreateCheckoutSessionCommand : IRequest<string>
{
    public required Guid ClientId { get; set; }
    public required Guid OrderId { get; set; }
    public Guid? CouponId { get; set; }
}