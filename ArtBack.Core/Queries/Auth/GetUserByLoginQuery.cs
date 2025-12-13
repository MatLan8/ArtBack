using ArtBack.Domain.Dtos;
using ArtBack.Domain.Entities;
using MediatR;

namespace ArtBack.Core.Queries.Auth;

public class GetUserByLoginQuery : IRequest<UserDto>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    
}