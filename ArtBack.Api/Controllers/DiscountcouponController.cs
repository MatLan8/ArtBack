using ArtBack.Core.Commands.DiscountCoupon;
using ArtBack.Core.Queries.DiscountCoupon;
using Microsoft.AspNetCore.Mvc;

namespace ArtBack.Api.Controllers;

public class DiscountcouponController : BaseController
{
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDiscountCouponQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById([FromQuery] GetByIdDiscountCouponQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateDiscountCouponCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    
    [HttpPatch("Update")]
    public async Task<IActionResult> Update(UpdateDiscountCouponCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch("Deactivate")]
    public async Task<IActionResult> Deactivate(DeactivateDiscountCouponCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    [HttpGet("calculate-automatic-discount")]
    public async Task<IActionResult> CalculateDiscount(
        [FromQuery] Guid clientId,
        [FromQuery] decimal cartTotal)
    {
        var result = await Mediator.Send(new CalculateAutomaticDiscountQuery
        {
            ClientId = clientId,
            CartTotal = cartTotal
        });

        return Ok(result);
    }

}