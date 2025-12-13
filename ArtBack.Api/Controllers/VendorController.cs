using ArtBack.Core.Queries.VendorReport;
using Microsoft.AspNetCore.Mvc;

namespace ArtBack.Api.Controllers;

public class VendorController : BaseController
{
    [HttpGet("SalesReport/{vendorId}")]
    public async Task<IActionResult> GetSalesReport(Guid vendorId)
    {
        var query = new GetVendorSalesReportQuery { VendorId = vendorId };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("ArtworksSalesDetails/{vendorId}")]
    public async Task<IActionResult> GetArtworksSalesDetails(Guid vendorId)
    {
        var query = new GetVendorArtworksSalesDetailsQuery { VendorId = vendorId };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("Stats/{vendorId}")]
    public async Task<IActionResult> GetStats(Guid vendorId)
    {
        var query = new GetVendorStatsQuery { VendorId = vendorId };
        var result = await Mediator.Send(query);
        return Ok(result);
    }
}
