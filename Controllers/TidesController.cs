using Microsoft.AspNetCore.Mvc;
using TideApi.Extensions;
using TideApi.Models;
using TideApi.Services;

namespace TideApi.Controllers;

[ApiController]
[Route("[Controller]")]
public class TidesController(ITideService tideService) : ControllerBase
{
    [HttpGet("crossing-times")]
    public async Task<ActionResult<GetThresholdCrossingsResponse>> GetThresholdCrossings(
        [FromQuery] DateTimeOffset start, DateTimeOffset end)
    {
        try
        {
            return Ok(await tideService.GetThresholdCrossings(start, end));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("crossing-status")]
    public async Task<ActionResult<GetCrossingStatusResponse>> GetCrossingStatus([FromQuery] DateTimeOffset requestedTime)
    {
        try
        {
            return Ok(await tideService.GetCrossingStatus(requestedTime));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("turning-points")]
    public async Task<ActionResult<GetTurningPointsResponse>> GetTideTurningPoints(
        [FromQuery] DateTimeOffset start, [FromQuery] DateTimeOffset end)
    {
        try
        {
            var turningPointsResponse = await tideService.GetTideTurningPoints(start, end);

            if (turningPointsResponse.TurningPoints!.Count != 0)
            {
                foreach (var turningPoint in turningPointsResponse.TurningPoints)
                {
                    turningPoint.TidePoint!.Time = turningPoint.TidePoint.Time!.ToUkTime();
                }
            }

            return Ok(turningPointsResponse);
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