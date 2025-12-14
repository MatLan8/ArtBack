using ArtBack.Core.Queries.Client;
using ArtBack.Domain.DTOs;
using ArtBack.Infrastructure; 
using MediatR;
using ArtBack.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Client;

public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientDto>
{
    private readonly ArtDbContext _context;

    public GetClientByIdQueryHandler(ArtDbContext context)
    {
        _context = context;
    }

    public async Task<ClientDto> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await _context.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ClientId, cancellationToken);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        return new ClientDto
        {
            FirstName = client.FirstName,
            LastName = client.LastName,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            Address = client.Address
        };
    }
}