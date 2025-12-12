using MediatR;

namespace ArtBack.Core.Commands.DiscountCoupon;

public class UpdateDiscountCouponCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public double DiscountAmount { get; set; }
    public DateTime BeginAt { get; set; }
    public DateTime ExpireAt { get; set; }
    public decimal StartingPrice { get; set; }
    public bool IsActive { get; set; }
}
