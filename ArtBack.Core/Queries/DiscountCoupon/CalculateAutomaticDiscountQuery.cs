using MediatR;

namespace ArtBack.Core.Queries.DiscountCoupon;

public class CalculateAutomaticDiscountQuery : IRequest<decimal>
{
    public Guid ClientId { get; set; }
    public decimal CartTotal { get; set; }
}