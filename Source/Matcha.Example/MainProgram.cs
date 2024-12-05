#region License Information (MIT)
// Matcha - A simple but neat logging library for .NET.
// Copyright (C) 2022 Analog Feelings
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using AnalogFeelings.Matcha.Enums;
using AnalogFeelings.Matcha.Sinks.Console;
using AnalogFeelings.Matcha.Sinks.Debugger;
using AnalogFeelings.Matcha.Sinks.File;

namespace AnalogFeelings.Matcha.Example;

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