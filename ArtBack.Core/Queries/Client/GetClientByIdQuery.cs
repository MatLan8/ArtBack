using MediatR;

namespace ArtBack.Core.Queries.Client;

public class GetClientByIdQuery : IRequest<ClientDto>
{
    public Guid ClientId { get; set; }
}