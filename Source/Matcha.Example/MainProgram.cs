using AnalogFeelings.Matcha;
using AnalogFeelings.Matcha.Enums;
using AnalogFeelings.Matcha.Sinks.Console;
using AnalogFeelings.Matcha.Sinks.Debugger;

namespace Matcha.Example;

file static class MainProgram
{
    private static ConsoleSinkConfig _Config = default!;
    private static DebuggerSinkConfig _ConfigDebug = default!;
    private static MatchaLogger _Logger = default!;

    public static async Task Main(string[] args)
    {
        _Config = new ConsoleSinkConfig()
        {
#if DEBUG
            SeverityFilterLevel = LogSeverity.Debug
#else
            SeverityFilterLevel = LogSeverity.Information
#endif
        };
        _ConfigDebug = new DebuggerSinkConfig()
        {
            SeverityFilterLevel = LogSeverity.Debug
        };

        ConsoleSink consoleSink = new ConsoleSink()
        {
            Config = _Config
        };
        DebuggerSink debuggerSink = new DebuggerSink()
        {
            Config = _ConfigDebug
        };
        
        _Logger = new MatchaLogger(consoleSink, debuggerSink);

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
        await _Logger.LogAsync(LogSeverity.Debug, "Hey, this is invisible in release mode!");
        await _Logger.LogAsync(LogSeverity.Information, "Hey, this is an information message!");
        await _Logger.LogAsync(LogSeverity.Success, "Hey, this is a success message!");
        await _Logger.LogAsync(LogSeverity.Warning, "Hey, this is a warning message!");
        await _Logger.LogAsync(LogSeverity.Error, "Hey, this is an error message!");
        await _Logger.LogAsync(LogSeverity.Fatal, "Hey, this is a fatal message!");
    }

    /// <summary>
    /// Shows off multiline formatting.
    /// </summary>
    private static async Task ShowOffMultiLine()
    {
        await _Logger.LogAsync(LogSeverity.Information, "Hey, this is a multi-line message!\nAs you can see, Matcha formats it!\nBe silly!");
        await _Logger.LogAsync(LogSeverity.Success, "Another multi-line message!\nBe sillier!! :3");
    }

    /// <summary>
    /// Shows off formatting.
    /// </summary>
    private static async Task ShowOffFormatting()
    {
        await _Logger.LogAsync(LogSeverity.Information, "We support formatting too! Here: {0}", int.MaxValue);
        await _Logger.LogAsync(LogSeverity.Information, "We support multiline formatting too! Here: {0}\nAnd here! {1}", ulong.MaxValue, ushort.MaxValue);
    }

    /// <summary>
    /// Shows off color toggling.
    /// </summary>
    private static async Task ShowOffColorToggling()
    {
        _Config.UseColors = false;

        await _Logger.LogAsync(LogSeverity.Information, "bye bye fancy colors!!");

        _Config.UseColors = true;

        await _Logger.LogAsync(LogSeverity.Information, "hello fancy colors!!");
    }

    /// <summary>
    /// Shows off date toggling.
    /// </summary>
    private static async Task ShowOffDateToggling()
    {
        _Config.OutputDate = false;
        _ConfigDebug.OutputDate = false;

        await _Logger.LogAsync(LogSeverity.Information, "bye bye date!!\nthis is so sad");

        _Config.OutputDate = true;
        _ConfigDebug.OutputDate = true;

        await _Logger.LogAsync(LogSeverity.Information, "hello date!!\nthis is so cool");
    }
}