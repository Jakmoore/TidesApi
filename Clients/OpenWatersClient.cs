using TideApi.Config;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;

namespace TideApi.Clients;

public class OpenWatersClient(
    OpenWatersConfig config,
    LocationConfig locationConfig,
    IHttpClientFactory httpClientFactory) : IOpenWatersClient
{
    public async Task<string> GetTidePoints(DateTimeOffset start, DateTimeOffset end)
    {
        var query = new Dictionary<string, string?>
        {
            ["latitude"] = locationConfig.Latitude.ToString(CultureInfo.InvariantCulture),
            ["longitude"] = locationConfig.Longitude.ToString(CultureInfo.InvariantCulture),
            ["start"] = start.ToString("O"),
            ["end"] = end.ToString("O")
        };

        var url = QueryHelpers.AddQueryString($"{config.BaseUrl}/tides/timeline", query);
        var client = httpClientFactory.CreateClient();

        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}