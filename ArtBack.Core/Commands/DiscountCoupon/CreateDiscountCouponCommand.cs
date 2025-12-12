using MediatR;

namespace ArtBack.Core.Commands.DiscountCoupon;

public class CreateDiscountCouponCommand : IRequest<Guid>
{
    public required string CouponCode { get; set; }
    public required string Description { get; set; }
    public required double DiscountAmount { get; set; }
    public required DateTime BeginAt { get; set; }
    public required DateTime ExpireAt { get; set; }
    public required decimal StartingPrice { get; set; }
}