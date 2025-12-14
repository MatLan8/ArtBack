using MediatR;

namespace ArtBack.Core.Commands.Order;

public class CreateOrderCommand : IRequest<Guid>
{
    public Guid ClientId { get; set; }
    public string? CouponCode { get; set; }
}