using MediatR;

namespace ArtBack.Core.Commands.DiscountCoupon;

public class UpdateDiscountCouponCommand : IRequest<Unit>
{
    public Guid Id { get; set; }

    public string? Description { get; set; }
    public double? DiscountAmount { get; set; }
    public DateTime? BeginAt { get; set; }
    public DateTime? ExpireAt { get; set; }
    public decimal? StartingPrice { get; set; }
    public bool? IsActive { get; set; }
}

