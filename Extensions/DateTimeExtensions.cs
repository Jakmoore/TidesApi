namespace TideApi.Extensions;

public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo _ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");

    public static DateTimeOffset ToUkTime(this DateTimeOffset dateTimeOffset)
    {
        return TimeZoneInfo.ConvertTime(dateTimeOffset, _ukTimeZone);
    }
}