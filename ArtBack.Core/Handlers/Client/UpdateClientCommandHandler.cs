using ArtBack.Core.Commands.Client;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ArtBack.Core.Handlers.Client;

public class UpdateClientCommandHandler
    : IRequestHandler<UpdateClientCommand, Unit>
{
    private readonly ArtDbContext _context;

    public UpdateClientCommandHandler(ArtDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        UpdateClientCommand request,
        CancellationToken cancellationToken)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);

        if (client == null)
            throw new Exception("Client not found");

        client.FirstName = request.Client.FirstName ?? client.FirstName;
        client.LastName = request.Client.LastName ?? client.LastName;
        client.Email = request.Client.Email ?? client.Email;
        client.PhoneNumber = request.Client.PhoneNumber ?? client.PhoneNumber;
        client.Address = request.Client.Address ?? client.Address;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}