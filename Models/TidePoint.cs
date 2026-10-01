using System.Text.Json.Serialization;

namespace TideApi.Models;

public class TidePoint
{
    [JsonPropertyName("time")]
    public DateTimeOffset Time { get; set; }

    [JsonPropertyName("level")]
    public double Level { get; set; }

    [JsonPropertyName("hour")]
    public double Hour { get; set; }
}