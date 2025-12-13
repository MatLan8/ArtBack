using ArtBack.Core.Commands.Artwork;
using ArtBack.Core.Commands.Auth;
using ArtBack.Domain.Entities;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers;

public class CreateUserCommandHandler(ArtDbContext dbContext) : IRequestHandler<CreateUserCommand, bool>
{
    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .Where(a => a.Username == request.Username).SingleOrDefaultAsync(cancellationToken);
        if(user != null) throw new Exception("User with that username already exists");
        
        
        if (request.Role == "Vendor")
        {
            var vendor = new Vendor
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Password = request.Password,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                ArtworkCount = 0,
                SoldArtworkCount = 0,
                Profits = 0
            };
            await dbContext.Vendors.AddAsync(vendor, cancellationToken);
        }
        else
        {
            var client = new Domain.Entities.Client
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Password = request.Password,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                OrderedArtworkCount = 0
            };
            await dbContext.Clients.AddAsync(client, cancellationToken);
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}