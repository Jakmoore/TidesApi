using Microsoft.AspNetCore.Mvc;
using TideApi.Models;
using TideApi.Services;

namespace TideApi.Controllers;

[ApiController]
[Route("[Controller]")]
public class TidesController(ITideService tideService) : ControllerBase
{
    [HttpGet("turning-points")]
    public async Task<ActionResult<GetTurningPointsResponse>> GetTideTurningPoints(
        [FromQuery] DateTimeOffset start, [FromQuery] DateTimeOffset end)
    {
        try
        {
            return Ok(await tideService.GetTideTurningPoints(start, end));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("state")]
    public async Task<ActionResult<TideState>> GetTideAtTime([FromQuery] DateTimeOffset requestedTime)
    {
        try
        {
            return Ok(await tideService.GetTideAtTime(requestedTime));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TidePoint>>> GetTidePoints(
        [FromQuery] DateTimeOffset start, [FromQuery] DateTimeOffset end)
    {
        try
        {
            return Ok(await tideService.GetTidePoints(start, end));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}