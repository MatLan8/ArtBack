using MediatR;
using ArtBack.Domain.DTOs;

namespace ArtBack.Core.Queries.Client;

public class GetClientByIdQuery : IRequest<ClientDto>
{
    public Guid ClientId { get; set; }
}