using ArtBack.Domain.Dtos;
using MediatR;
using ArtBack.Domain.Entities;

namespace ArtBack.Core.Queries.Client;

public class GetClientOrdersQuery : IRequest<List<OrderDto>>
{
    public Guid ClientId { get; set; }
}