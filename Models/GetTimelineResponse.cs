using System.Text.Json.Serialization;

namespace TideApi.Models;

public class GetTimeLineResponse
{
    [JsonPropertyName("timeline")]
    public required IEnumerable<TidePoint> Timeline { get; set; }
}