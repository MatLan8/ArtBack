using ArtBack.Core.Queries.Auth;
using ArtBack.Domain.Dtos;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers;

public class GetUserByLoginQueryHandler(ArtDbContext dbContext) : IRequestHandler<GetUserByLoginQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByLoginQuery request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .Where(a => a.Username == request.Username).SingleOrDefaultAsync(cancellationToken);
        if(user == null) throw new Exception("User not found");

        if (user.Password != request.Password) throw new Exception("Wrong password");
        
        var vendor = await dbContext.Vendors.Where(v => v.Id == user.Id).SingleOrDefaultAsync(cancellationToken);
        string role;
        if (vendor != null)
        {
            role = "Vendor";
        }
        else
        {
            role = "Client";
        }
        
        var userDto = new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            Role = role
        };
        return userDto; 
    }
}