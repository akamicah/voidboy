namespace DirectoryService.Shared.Extensions;

public static class DateTimeExtensions
{
    private const long EpochTicks = 621355968000000000;
    private const long TicksPeriodMs = 10000;

    public static readonly DateTime Epoch = new DateTime(EpochTicks, DateTimeKind.Utc);

    public static long ToMilliSecondsTimestamp(this DateTime date)
    {
        var ts = (date.Ticks - EpochTicks) / TicksPeriodMs;
        return ts;
    }

    public static string ToUniversalIso8601(this DateTime dateTime)
    {
        return dateTime.ToUniversalTime().ToString("u").Replace(" ", "T");
    }
}