using ArtBack.Core.Commands.Auth;
using ArtBack.Core.Queries.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ArtBack.Api.Controllers;

public class AuthController : BaseController
{
      [HttpGet("Login")]
      public async Task<IActionResult> Login([FromQuery] GetUserByLoginQuery query)
      {
       var result = await Mediator.Send(query);
       return Ok(result);
      }
      
      [HttpPost("Register")]
      public async Task<IActionResult> Register(CreateUserCommand command)
      {
          var result = await Mediator.Send(command);
          return Ok(result);
      }
}