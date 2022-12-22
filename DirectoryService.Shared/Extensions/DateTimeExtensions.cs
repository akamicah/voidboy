namespace DirectoryService.Shared.Extensions;

// Author: Cybermaxs (https://gist.github.com/Cybermaxs/e142bc79c551b907a39a)

public static class DateTimeExtensions
{
    public const long EpochTicks = 621355968000000000;
    public const long TicksPeriod = 10000000;
    public const long TicksPeriodMs = 10000;

    //epoch time
    public static readonly DateTime Epoch = new DateTime(EpochTicks, DateTimeKind.Utc);

    /// <summary>
    /// Number of milliseconds since epoch(1/1/1970).
    /// </summary>
    /// <param name="date">DateTime to convert</param>
    /// <returns>Number of milliseconds since 1/1/1970 (Unix timestamp)</returns>
    public static long ToMilliSecondsTimestamp(this DateTime date)
    {
        long ts = (date.Ticks - EpochTicks) / TicksPeriodMs;
        return ts;
    }

    /// <summary>
    /// Number of seconds since epoch(1/1/1970).
    /// </summary>
    /// <param name="date">DateTime to convert</param>
    /// <returns>Number of seconds since 1/1/1970 (Unix timestamp)</returns>
    public static long ToSecondsTimestamp(this DateTime date)
    {
        long ts = (date.Ticks - EpochTicks) / TicksPeriod;
        return ts;
    }

    /// <summary>
    /// Round a timestamp in seconds.
    /// </summary>
    /// <param name="date">DateTime to convert</param>
    /// <param name="factor">Round factor in seconds.</param>
    /// <returns>Rounded Timestamp in seconds.</returns>
    public static long ToRoundedSecondsTimestamp(this DateTime date, long factor)
    {
        return ((long)date.ToSecondsTimestamp() / factor) * factor;
    }
}