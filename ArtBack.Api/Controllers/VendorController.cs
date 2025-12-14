using ArtBack.Core.Queries.VendorReport;
using Microsoft.AspNetCore.Mvc;

namespace ArtBack.Api.Controllers;

public class VendorController : BaseController
{
    [HttpGet("SalesReport")]
    public async Task<IActionResult> GetSalesReport([FromQuery] GetVendorSalesReportQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("ArtworksSalesDetails")]
    public async Task<IActionResult> GetArtworksSalesDetails([FromQuery] GetVendorArtworksSalesDetailsQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("Stats")]
    public async Task<IActionResult> GetStats([FromQuery] GetVendorStatsQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
}
