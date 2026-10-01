using System.Text.Json;
using TideApi.Clients;
using TideApi.Models;

namespace TideApi.Services;

public class TideService(ILogger<TideService> logger, IOpenWatersClient openWatersClient) : ITideService
{
    public async Task<GetTurningPointsResponse> GetTideTurningPoints(DateTimeOffset start, DateTimeOffset end)
    {
        var tidePoints = await GetTidePoints(start, end);
        var turningPoints = GetTurningPoints([.. tidePoints]);

        if (logger.IsEnabled(LogLevel.Debug) && turningPoints.Count != 0)
        {
            foreach (var point in turningPoints)
            {
                logger.LogDebug(
                    "Turning point: Time: {Time} Level: {Level} Status: {Status}",
                    point.TidePoint!.Time,
                    point.TidePoint.Level,
                    point.Type);
            }
        }

        return new GetTurningPointsResponse(turningPoints);
    }

    public async Task<TideState> GetTideAtTime(DateTimeOffset requestedTime)
    {
        // Get points either side of requested so we can calculate ebb/flood
        var start = requestedTime.AddMinutes(-20);
        var end = requestedTime.AddMinutes(20);

        var tidePoints = await GetTidePoints(start, end);

        TidePoint? previous = null;
        TidePoint? next = null;

        foreach (var tidePoint in tidePoints)
        {
            if (tidePoint.Time >= requestedTime)
            {
                next = tidePoint;
                break;
            }
            else
            {
                previous = tidePoint;
            }
        }

        if (previous is null || next is null)
        {
            throw new InvalidOperationException("Unable to retrieve required tide points");
        }

        return new TideState(requestedTime, GetLevelAtRequestedTime(previous, next, requestedTime),
            IsRising(previous.Level, next.Level));
    }

    public async Task<IEnumerable<TidePoint>> GetTidePoints(DateTimeOffset start, DateTimeOffset end)
    {
        var response = await openWatersClient.GetTidePoints(start, end);

        var timelineResponse = JsonSerializer.Deserialize<GetTimeLineResponse>(response) ??
         throw new InvalidOperationException("Failed to deserialize timeline response");

        return timelineResponse.Timeline;
    }

    private static double GetLevelAtRequestedTime(TidePoint previous, TidePoint next, DateTimeOffset requestedTime)
    {
        var totalWindowMins = (next.Time - previous.Time).TotalMinutes;
        var requestedFromPreviousMins = (requestedTime - previous.Time).TotalMinutes;
        var progression = requestedFromPreviousMins / totalWindowMins;
        var levelDifference = next.Level - previous.Level;

        return previous.Level + (levelDifference * progression);
    }

    private static bool IsRising(double startLevel, double endLevel) => startLevel < endLevel;

    // Itterate 3 points at a time to find turning points from Open Waters data
    private static List<TideTurningPoint> GetTurningPoints(List<TidePoint> tidePoints)
    {
        List<TideTurningPoint> turningPoints = [];

        for (int i = 1; i <= tidePoints.Count - 2; i++)
        {
            var previous = tidePoints[i - 1];
            var current = tidePoints[i];
            var next = tidePoints[i + 1];

            if (CalculateTurningPoint(previous, current, next) is TideTurningPoint tideTurningPoint)
            {
                turningPoints.Add(tideTurningPoint);
            }
        }

        return turningPoints;
    }

    private static TideTurningPoint? CalculateTurningPoint(TidePoint previous, TidePoint current, TidePoint next)
    {
        if (current.Level > previous.Level && current.Level > next.Level)
        {
            return new TideTurningPoint(current, TideTurningPointType.HIGH);
        }

        if (current.Level < previous.Level && current.Level < next.Level)
        {
            return new TideTurningPoint(current, TideTurningPointType.LOW);
        }

        return null;
    }
}
