using MediatR;
using ArtBack.Domain.Dtos;

public class UpdateClientCommand : IRequest<Unit>
{
    public Guid ClientId { get; set; }
    public ClientDto Client { get; set; }
}