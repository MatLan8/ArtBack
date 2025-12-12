using MediatR;
using ArtBack.Domain.Dtos;

namespace ArtBack.Core.Queries.DiscountCoupon;

public class ValidateDiscountCouponQuery : IRequest<ValidateDiscountCouponDto?>
{
    public string Code { get; set; } = null!;
    public decimal CartTotal { get; set; }
}