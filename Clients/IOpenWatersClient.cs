namespace TideApi.Clients;

public interface IOpenWatersClient
{
    Task<string> GetTidePoints(DateTimeOffset start, DateTimeOffset end);
}