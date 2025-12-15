using ArtBack.Domain.Dtos;
using MediatR;

namespace ArtBack.Core.Commands.Client;

public class UpdateClientCommand : IRequest<Unit>
{
    public Guid ClientId { get; set; }
    public ClientDto Client { get; set; }
}