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

using System.Runtime.CompilerServices;
using System.Text;
using AnalogFeelings.Matcha.Enums;
using AnalogFeelings.Matcha.Extensions;
using AnalogFeelings.Matcha.Interfaces;
using AnalogFeelings.Matcha.Models;

namespace AnalogFeelings.Matcha.Sinks.Console;

using Console = System.Console;
using SeverityData = (string Header, string HeaderColor, string TextColor);

/// <summary>
/// A simple sink to output logs to the console.
/// </summary>
public sealed class ConsoleSink : IMatchaSink<ConsoleSinkConfig>, IDisposable
{
    /// <inheritdoc/>
    public required ConsoleSinkConfig Config { get; init; }

    /// <summary>
    /// Dictionary to quickly convert a severity to a string and its color.
    /// </summary>
    private readonly Dictionary<LogSeverity, SeverityData> _severityDict = new Dictionary<LogSeverity, SeverityData>()
    {
        [LogSeverity.Debug] = ("DBG", ColorConstants.PINK, ColorConstants.LIGHT_PINK),
        [LogSeverity.Information] = ("INF", ColorConstants.CYAN, ColorConstants.LIGHT_BLUE),
        [LogSeverity.Success] = ("SCS", ColorConstants.GREEN, ColorConstants.LIGHT_GREEN),
        [LogSeverity.Warning] = ("WRN", ColorConstants.YELLOW, ColorConstants.LIGHT_YELLOW),
        [LogSeverity.Error] = ("ERR", ColorConstants.RED, ColorConstants.LIGHT_RED),
        [LogSeverity.Fatal] = ("FTL", ColorConstants.ORANGE, ColorConstants.BROWN)
    };

    /// <summary>
    /// Provides a semaphore to prevent race conditions.
    /// </summary>
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    // Reduces GC pressure.
    private readonly StringBuilder _fullBuilder = new StringBuilder();
    private readonly StringBuilder _indentBuilder = new StringBuilder();

    /// <inheritdoc/>
    public void InitializeSink()
    {
        // Empty.
    }

    /// <inheritdoc/>
    public async Task WriteLogAsync(LogEntry entry)
    {
        await _semaphore.WaitAsync();

        try
        {
            int logHeaderLength = 1;

            if (Config.UseColors)
            {
                _fullBuilder.Append(ColorConstants.WHITE);
                _fullBuilder.Append('[');
                _fullBuilder.Append(ColorConstants.ANSI_RESET);
            }
            else
            {
                _fullBuilder.Append('[');
            }

            if (Config.OutputDate)
            {
                string date = entry.Time.ToString(Config.DateFormat);

                if (Config.UseColors)
                {
                    _fullBuilder.Append(ColorConstants.LIGHT_GRAY);
                    _fullBuilder.Append(date);
                    _fullBuilder.Append(ColorConstants.ANSI_RESET);
                }
                else
                {
                    _fullBuilder.Append(date);
                }

                _fullBuilder.Append(' ');

                // Account for the space.
                logHeaderLength += date.Length + 1;
            }

            SeverityData data = _severityDict[entry.Severity];

            if (Config.UseColors)
            {
                _fullBuilder.Append(data.HeaderColor);
                _fullBuilder.Append(data.Header);
                _fullBuilder.Append(ColorConstants.ANSI_RESET);

                _fullBuilder.Append(ColorConstants.WHITE);
                _fullBuilder.Append(']');
                _fullBuilder.Append(ColorConstants.ANSI_RESET);
            }
            else
            {
                _fullBuilder.Append(data.Header);
                _fullBuilder.Append(']');
            }

            logHeaderLength += data.Header.Length + 1;

            string[] splittedContent = entry.Content.SplitLines();

            Common.GenerateIndents(splittedContent.Length, logHeaderLength, Config.UseColors, _indentBuilder, out string indentMiddle, out string indentLast);

            for (int i = 0; i < splittedContent.Length; i++)
            {
                string contentLine = splittedContent[i];

                // Are we on the second line?
                if (i == 1)
                {
                    // Theres only 2 lines, put the end already.
                    if (splittedContent.Length == 2)
                        _fullBuilder.Append(indentLast);
                    else if (splittedContent.Length > 2)
                        _fullBuilder.Append(indentMiddle);
                }
                else if (i > 1)
                {
                    // We are on the last line!
                    if (i == splittedContent.Length - 1)
                        _fullBuilder.Append(indentLast);
                    else
                        _fullBuilder.Append(indentMiddle);
                }

                _fullBuilder.Append(' ');

                if (Config.UseColors)
                {
                    _fullBuilder.Append(data.TextColor);
                    _fullBuilder.Append(contentLine);
                    _fullBuilder.Append(ColorConstants.ANSI_RESET);
                }
                else
                {
                    _fullBuilder.Append(contentLine);
                }

                _fullBuilder.AppendLine();
            }

            Console.Write(_fullBuilder.ToString());
        }
        finally
        {
            _fullBuilder.Clear();
            _indentBuilder.Clear();

            _semaphore.Release();
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _semaphore.Dispose();
    }
}