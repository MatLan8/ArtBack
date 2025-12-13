using ArtBack.Domain.Dtos;
using MediatR;

namespace ArtBack.Core.Queries.DiscountCoupon;

public class GetAllDiscountCouponQuery : IRequest<List<DiscountCouponDto>>;
