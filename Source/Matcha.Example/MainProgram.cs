using AnalogFeelings.Matcha;
using AnalogFeelings.Matcha.Enums;
using AnalogFeelings.Matcha.Sinks.Console;
using AnalogFeelings.Matcha.Sinks.Debugger;
using AnalogFeelings.Matcha.Sinks.File;

namespace Matcha.Example;

file static class MainProgram
{
    private static ConsoleSinkConfig _config = default!;
    private static DebuggerSinkConfig _configDebug = default!;
    private static FileSinkConfig _configFile = default!;
    private static MatchaLogger _logger = default!;

    public static async Task Main(string[] args)
    {
        _config = new ConsoleSinkConfig()
        {
#if DEBUG
            SeverityFilterLevel = LogSeverity.Debug
#else
            SeverityFilterLevel = LogSeverity.Information
#endif
        };
        _configDebug = new DebuggerSinkConfig()
        {
            SeverityFilterLevel = LogSeverity.Debug
        };
        _configFile = new FileSinkConfig()
        {
#if DEBUG
            SeverityFilterLevel = LogSeverity.Debug,
#else
            SeverityFilterLevel = LogSeverity.Information,
#endif
            Overwrite = true
        };

        ConsoleSink consoleSink = new ConsoleSink()
        {
            Config = _config
        };
        DebuggerSink debuggerSink = new DebuggerSink()
        {
            Config = _configDebug
        };
        FileSink fileSink = new FileSink()
        {
            Config = _configFile
        };
        
        _logger = new MatchaLogger(consoleSink, debuggerSink, fileSink);

        await ShowOffSeverities();
        await ShowOffMultiLine();
        await ShowOffFormatting();

        await ShowOffColorToggling();

        await ShowOffDateToggling();
    }

    /// <summary>
    /// Shows off the different severities.
    /// </summary>
    private static async Task ShowOffSeverities()
    {
        await _logger.LogAsync(LogSeverity.Debug, "Hey, this is invisible in release mode!");
        await _logger.LogAsync(LogSeverity.Information, "Hey, this is an information message!");
        await _logger.LogAsync(LogSeverity.Success, "Hey, this is a success message!");
        await _logger.LogAsync(LogSeverity.Warning, "Hey, this is a warning message!");
        await _logger.LogAsync(LogSeverity.Error, "Hey, this is an error message!");
        await _logger.LogAsync(LogSeverity.Fatal, "Hey, this is a fatal message!");
    }

    /// <summary>
    /// Shows off multiline formatting.
    /// </summary>
    private static async Task ShowOffMultiLine()
    {
        await _logger.LogAsync(LogSeverity.Information, "Hey, this is a multi-line message!\nAs you can see, Matcha formats it!\nBe silly!");
        await _logger.LogAsync(LogSeverity.Success, "Another multi-line message!\nBe sillier!! :3");
    }

    /// <summary>
    /// Shows off formatting.
    /// </summary>
    private static async Task ShowOffFormatting()
    {
        await _logger.LogAsync(LogSeverity.Information, "We support formatting too! Here: {0}", int.MaxValue);
        await _logger.LogAsync(LogSeverity.Information, "We support multiline formatting too! Here: {0}\nAnd here! {1}", ulong.MaxValue, ushort.MaxValue);
    }

    /// <summary>
    /// Shows off color toggling.
    /// </summary>
    private static async Task ShowOffColorToggling()
    {
        _config.UseColors = false;

        await _logger.LogAsync(LogSeverity.Information, "bye bye fancy colors!!");

        _config.UseColors = true;

        await _logger.LogAsync(LogSeverity.Information, "hello fancy colors!!");
    }

    /// <summary>
    /// Shows off date toggling.
    /// </summary>
    private static async Task ShowOffDateToggling()
    {
        _config.OutputDate = false;
        _configDebug.OutputDate = false;
        _configFile.OutputDate = false;

        await _logger.LogAsync(LogSeverity.Information, "bye bye date!!\nthis is so sad");

        _config.OutputDate = true;
        _configDebug.OutputDate = true;
        _configFile.OutputDate = true;

        await _logger.LogAsync(LogSeverity.Information, "hello date!!\nthis is so cool");
    }
}