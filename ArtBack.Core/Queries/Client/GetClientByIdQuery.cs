using MediatR;
using ArtBack.Domain.Dtos;

namespace ArtBack.Core.Queries.Client;

public class GetClientByIdQuery : IRequest<ClientDto>
{
    public Guid ClientId { get; set; }
}