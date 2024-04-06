using AnalogFeelings.Matcha;
using AnalogFeelings.Matcha.Enums;
using AnalogFeelings.Matcha.Sinks.Console;

namespace Matcha.Example;

file static class MainProgram
{
    public static async Task Main(string[] args)
    {
        ConsoleSinkConfig config = new ConsoleSinkConfig()
        {
#if DEBUG
            SeverityFilterLevel = LogSeverity.Debug,
#else
            SeverityFilterLevel = LogSeverity.Information,
#endif
        };

        MatchaLogger logger = new MatchaLogger(new ConsoleSink()
        {
            Config = config
        });
        
        await logger.LogAsync(LogSeverity.Debug, "Hey, this is invisible in release mode!");
        await logger.LogAsync(LogSeverity.Information, "Hey, this is an information message!");
        await logger.LogAsync(LogSeverity.Success, "Hey, this is a success message!");
        await logger.LogAsync(LogSeverity.Warning, "Hey, this is a warning message!");
        await logger.LogAsync(LogSeverity.Error, "Hey, this is an error message!");
        await logger.LogAsync(LogSeverity.Fatal, "Hey, this is a fatal message!");
        await logger.LogAsync(LogSeverity.Information, "Hey, this is a multi-line message!\nAs you can see, Matcha formats it!\nBe silly!");
        
        logger.Log(LogSeverity.Information, "Sync wrappers also exist, this is a sync call to Log()!");
    }
}