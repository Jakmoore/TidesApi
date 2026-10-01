using System.Text.Json.Serialization;

namespace TideApi.Models;

public record GetTurningPointsResponse([property: JsonPropertyName("turningPoints")] List<TideTurningPoint>? TurningPoints);
