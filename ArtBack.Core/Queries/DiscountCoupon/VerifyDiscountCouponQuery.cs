using ArtBack.Domain.Dtos;
using MediatR;

namespace ArtBack.Core.Queries.DiscountCoupon;

public class VerifyDiscountCouponQuery : IRequest<DiscountCouponVerificationDto>
{
    public required string DiscountCode { get; set; }
    public required Guid ClientId { get; set; }
}