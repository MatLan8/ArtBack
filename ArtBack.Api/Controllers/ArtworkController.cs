using ArtBack.Core.Commands.Artwork;
using ArtBack.Core.Queries.Artwork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtBack.Api.Controllers;

public class ArtworkController : BaseController
{
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllArtworksQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById([FromQuery] GetByIdArtworkQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("GetAllByVendorId")]
    public async Task<IActionResult> GetAllByVendorId([FromQuery] GetAllArtworksByVendorIdQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateArtworkCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    
    [HttpPatch("Update")]
    public async Task<IActionResult> Update(UpdateArtWorkCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPatch("Remove")]
    public async Task<IActionResult> Remove(RemoveArtworkCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    

    
    
    
}