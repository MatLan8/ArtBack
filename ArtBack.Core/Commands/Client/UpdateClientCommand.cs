using MediatR;
using ArtBack.Domain.DTOs;

public class UpdateClientCommand : IRequest<Unit>
{
    public Guid ClientId { get; set; }
    public ClientDto Client { get; set; }
}