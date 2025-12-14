using MediatR;

namespace ArtBack.Core.Queries.Cart;

public class GetAutomaticCartDiscountQuery : IRequest<double>
{
    public required Guid ClientId { get; set; }
}