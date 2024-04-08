using AnalogFeelings.Matcha;
using AnalogFeelings.Matcha.Enums;
using AnalogFeelings.Matcha.Sinks.Console;

namespace Matcha.Example;

file static class MainProgram
{
    private static ConsoleSinkConfig Config;
    private static MatchaLogger Logger;

    public static async Task Main(string[] args)
    {
        Config = new ConsoleSinkConfig()
        {
#if DEBUG
            SeverityFilterLevel = LogSeverity.Debug,
#else
            SeverityFilterLevel = LogSeverity.Information,
#endif
        };
        Logger = new MatchaLogger(new ConsoleSink()
        {
            Config = Config
        });

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
        await Logger.LogAsync(LogSeverity.Debug, "Hey, this is invisible in release mode!");
        await Logger.LogAsync(LogSeverity.Information, "Hey, this is an information message!");
        await Logger.LogAsync(LogSeverity.Success, "Hey, this is a success message!");
        await Logger.LogAsync(LogSeverity.Warning, "Hey, this is a warning message!");
        await Logger.LogAsync(LogSeverity.Error, "Hey, this is an error message!");
        await Logger.LogAsync(LogSeverity.Fatal, "Hey, this is a fatal message!");
    }

    /// <summary>
    /// Shows off multiline formatting.
    /// </summary>
    private static async Task ShowOffMultiLine()
    {
        await Logger.LogAsync(LogSeverity.Information, "Hey, this is a multi-line message!\nAs you can see, Matcha formats it!\nBe silly!");
        await Logger.LogAsync(LogSeverity.Success, "Another multi-line message!\nBe sillier!! :3");
    }

    /// <summary>
    /// Shows off formatting.
    /// </summary>
    private static async Task ShowOffFormatting()
    {
        await Logger.LogAsync(LogSeverity.Information, "We support formatting too! Here: {0}", int.MaxValue);
        await Logger.LogAsync(LogSeverity.Information, "We support multiline formatting too! Here: {0}\nAnd here! {1}", ulong.MaxValue, ushort.MaxValue);
    }

    /// <summary>
    /// Shows off color toggling.
    /// </summary>
    private static async Task ShowOffColorToggling()
    {
        Config.UseColors = false;

        await Logger.LogAsync(LogSeverity.Information, "bye bye fancy colors!!");

        Config.UseColors = true;

        await Logger.LogAsync(LogSeverity.Information, "hello fancy colors!!");
    }

    /// <summary>
    /// Shows off date toggling.
    /// </summary>
    private static async Task ShowOffDateToggling()
    {
        Config.OutputDate = false;

        await Logger.LogAsync(LogSeverity.Information, "bye bye date!!\nthis is so sad");

        Config.OutputDate = true;

        await Logger.LogAsync(LogSeverity.Information, "hello date!!\nthis is so cool");
    }
}