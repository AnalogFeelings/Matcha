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
    private readonly IMatchaSink[] _sinks;

    /// <summary>
    /// Creates a new instance of Matcha.
    /// </summary>
    /// <param name="sinks">The logging sinks to use.</param>
    public MatchaLogger(params IMatchaSink[] sinks)
    {
        _sinks = sinks;
    }

    /// <summary>
    /// Logs a message with a severity to all sinks.
    /// </summary>
    /// <param name="severity">The severity of the log message.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="format">The string format parameters.</param>
    /// <exception cref="ArgumentException">Thrown if <see cref="message"/> is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <see cref="message"/> is null.</exception>
    public void Log(LogSeverity severity, string message, params object[]? format) =>
        LogAsync(severity, message, format).GetAwaiter().GetResult();

    /// <summary>
    /// Logs a message with a severity to all sinks asynchronously.
    /// </summary>
    /// <param name="severity">The severity of the log message.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="format">The string format parameters.</param>
    /// <exception cref="ArgumentException">Thrown if <see cref="message"/> is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <see cref="message"/> is null.</exception>
    public async Task LogAsync(LogSeverity severity, string message, params object[]? format)
    {
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));

        if (_sinks.Length == 0)
            return;

        List<Task> taskList = new List<Task>();
        LogEntry entry = new LogEntry()
        {
            Content = message,
            Severity = severity,
            Time = DateTime.Now,
            Format = format
        };

        foreach (IMatchaSink sink in _sinks)
        {
            taskList.Add(sink.WriteLogAsync(entry));
        }

        await Task.WhenAll(taskList);
    }
    
    /// <inheritdoc/>
    public void Dispose()
    {
        foreach (IMatchaSink sink in _sinks)
        {
            (sink as IDisposable)?.Dispose();
        }
    }
}