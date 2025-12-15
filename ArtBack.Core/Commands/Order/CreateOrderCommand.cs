using ArtBack.Domain.Types;
using MediatR;

namespace ArtBack.Core.Commands.Order;

public class CreateOrderCommand : IRequest<Guid>
{
    
    public required Guid ClientId { get; set; }
    public required string DeliveryAddress { get; set; }
    public required string Comment { get; set; }
    
    public required DeliveryMethod DeliveryMethod { get; set; }
    
    public Guid? DiscountCouponId { get; set; }
}