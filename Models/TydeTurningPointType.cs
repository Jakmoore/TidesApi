using System.Text.Json.Serialization;

namespace TideApi.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TideTurningPointType
{
    HIGH,
    LOW
}