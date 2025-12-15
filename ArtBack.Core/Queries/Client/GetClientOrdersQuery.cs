using MediatR;
using ArtBack.Domain.Entities;

namespace ArtBack.Core.Queries.Client;

public class GetClientOrdersQuery : IRequest<List<Order>>
{
    public Guid ClientId { get; set; }
}