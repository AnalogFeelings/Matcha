#region License Information (MIT)
// Matcha - A simple but neat logging library for .NET.
// Copyright (C) 2022-2024 Analog Feelings
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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using AnalogFeelings.Matcha.Enums;
using AnalogFeelings.Matcha.Interfaces;
using AnalogFeelings.Matcha.Models;

namespace AnalogFeelings.Matcha;

/// <summary>
/// The core class of Matcha, provides all of the library's
/// logging functionality.
/// </summary>
public sealed class MatchaLogger : IDisposable
{
    /// <summary>
    /// An array of the currently used logging sinks.
    /// </summary>
    private readonly IMatchaSink<SinkConfig>[] _sinks;

    /// <summary>
    /// Creates a new instance of Matcha.
    /// </summary>
    /// <param name="sinks">The logging sinks to use.</param>
    public MatchaLogger(params IMatchaSink<SinkConfig>[] sinks)
    {
        _sinks = sinks;

        foreach (IMatchaSink<SinkConfig> sink in _sinks)
        {
            sink.InitializeSink();
        }
    }

    /// <summary>
    /// Logs a message with a severity to all sinks.
    /// </summary>
    /// <param name="severity">The severity of the log message.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="format">The string format parameters.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="message"/> is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Log(LogSeverity severity, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object?[]? format) =>
        LogAsync(severity, message, format).GetAwaiter().GetResult();

    /// <summary>
    /// Logs a message with a severity to all sinks asynchronously.
    /// </summary>
    /// <param name="severity">The severity of the log message.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="format">The string format parameters.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="message"/> is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is null.</exception>
    public async Task LogAsync(LogSeverity severity, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object?[]? format)
    {
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));

        if (_sinks.Length == 0)
            return;

        object?[] formatArray = format ?? Array.Empty<object>();
        string formattedMessage = string.Format(message, formatArray);

        // Check if at least one sink is enabled or usable.
        bool passedTests = false;
        Task?[] taskArray = new Task[_sinks.Length];
        LogEntry entry = new LogEntry()
        {
            Content = formattedMessage,
            Severity = severity,
            Time = DateTime.Now
        };

        for (int i = 0; i < _sinks.Length; i++)
        {
            IMatchaSink<SinkConfig> sink = _sinks[i];

            if (!sink.Config.Enabled)
                continue;
            if ((int)sink.Config.SeverityFilterLevel > (int)severity)
                continue;

            taskArray[i] = sink.WriteLogAsync(entry);
            passedTests = true;
        }

        if (!passedTests)
            return;

        await Task.WhenAll(taskArray.Where(x => x != null)!);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        foreach (IMatchaSink<SinkConfig> sink in _sinks)
        {
            (sink as IDisposable)?.Dispose();
        }
    }
}