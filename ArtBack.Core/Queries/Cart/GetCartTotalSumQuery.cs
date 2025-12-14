using MediatR;

namespace ArtBack.Core.Queries.Cart;

public class GetCartTotalSumQuery : IRequest<decimal>
{
    public required Guid clientId { get; set; }
}