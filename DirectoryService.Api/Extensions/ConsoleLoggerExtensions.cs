using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace DirectoryService.Api.Extensions;

public static class ConsoleLoggerExtensions
{
    public static ILoggingBuilder AddCustomFormatter(
        this ILoggingBuilder builder) =>
        builder.AddConsole(options => options.FormatterName = nameof(OverteLoggingFormatter))
            .AddConsoleFormatter<OverteLoggingFormatter, ConsoleFormatterOptions>();
}

public sealed class OverteLoggingFormatter : ConsoleFormatter, IDisposable
{
    private readonly IDisposable? _optionsReloadToken;
    private ConsoleFormatterOptions _formatterOptions;

    public OverteLoggingFormatter(IOptionsMonitor<ConsoleFormatterOptions> options) : base(nameof(OverteLoggingFormatter)) =>
        (_optionsReloadToken, _formatterOptions) =
        (options.OnChange(ReloadLoggerOptions), options.CurrentValue);

    private void ReloadLoggerOptions(ConsoleFormatterOptions options) =>
        _formatterOptions = options;

    public override void Write<TState>(
        in LogEntry<TState> logEntry,
        IExternalScopeProvider scopeProvider,
        TextWriter textWriter)
    {
        var message =
            logEntry.Formatter?.Invoke(
                logEntry.State, logEntry.Exception);

        if (message is null)
        {
            return;
        }

        var level = logEntry.LogLevel switch
        {
            LogLevel.Warning => "Warn",
            LogLevel.Error => "Error",
            LogLevel.Critical => "Critical",
            LogLevel.Debug => "Debug",
            _ => ""
        };

        level = level.PadLeft(10);
        textWriter.WriteLine($"Overte {level} | {message}");
    }
    public void Dispose() => _optionsReloadToken?.Dispose();
}