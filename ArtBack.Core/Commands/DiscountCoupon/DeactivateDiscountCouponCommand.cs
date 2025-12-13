using MediatR;

namespace ArtBack.Core.Commands.DiscountCoupon;

public class DeactivateDiscountCouponCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}