using ArtBack.Domain.Dtos;
using MediatR;

namespace ArtBack.Core.Queries.DiscountCoupon;

public class GetByIdDiscountCouponQuery : IRequest<DiscountCouponDto>
{
    public  Guid Id { get; set; }
}