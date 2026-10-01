using TideApi.Models;

namespace TideApi.Services;

public interface ITideService
{
    Task<GetTurningPointsResponse> GetTideTurningPoints(DateTimeOffset start, DateTimeOffset end);
    Task<TideState> GetTideAtTime(DateTimeOffset requestedTime);
    Task<IEnumerable<TidePoint>> GetTidePoints(DateTimeOffset start, DateTimeOffset end);
}